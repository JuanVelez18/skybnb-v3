using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using application.DTOs;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace presentations
{
    public class Comunication
    {
        private string _host = string.Empty;
        private string? _accessToken;
        private string? _refreshToken;

        public Comunication(IOptions<PresentationConfiguration>? options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options cannot be null.");

            if (string.IsNullOrEmpty(options.Value.Host))
                throw new ArgumentNullException(nameof(options.Value.Host), "Host cannot be null or empty.");

            _host = options.Value.Host;
        }

        public class Response<T>
        {
            public bool Ok { get; init; }
            public T? Data { get; init; }
            public ProblemDetails? Error { get; init; }
        }

        private async Task<Response<T>> RefreshAndRetry<T, D>(string endpoint, D? data) where D : class
        {
            if (string.IsNullOrEmpty(_refreshToken))
            {
                throw new InvalidOperationException("Refresh token is not set.");
            }

            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            var refreshMessage = await httpClient.PostAsJsonAsync($"{_host}/auth/refresh", new RefreshTokenDto
            {
                RefreshToken = _refreshToken
            });

            if (!refreshMessage.IsSuccessStatusCode)
            {
                return new Response<T>
                {
                    Ok = false,
                    Error = new ProblemDetails
                    {
                        Title = "Login Required",
                        Detail = "Please log in again."
                    }
                };
            }

            var content = await refreshMessage.Content.ReadAsStringAsync();
            var tokens = JsonSerializer.Deserialize<TokensDto>(content)!;
            _accessToken = tokens.AccessToken;
            _refreshToken = tokens.RefreshToken;

            return await Execute<T, D>(endpoint, data);
        }

        public async Task<Response<T>> Execute<T>(string endpoint) where T : class
        {
            return await Execute<T, object>(endpoint, null);
        }
        public async Task<Response<T>> Execute<T, D>(string endpoint, D? data) where D : class
        {
            Response<T> response;
            var url = $"{_host}{endpoint}";

            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            if (!string.IsNullOrEmpty(_accessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            try
            {
                var message = data == null
                    ? await httpClient.GetAsync(url)
                    : await httpClient.PostAsJsonAsync(url, data);

                var content = await message.Content.ReadAsStringAsync();


                if (message.StatusCode == HttpStatusCode.Unauthorized && content.Contains("token expired", StringComparison.CurrentCultureIgnoreCase))
                {
                    return await RefreshAndRetry<T, D>(endpoint, data);
                }

                if (!message.IsSuccessStatusCode)
                {
                    response = new Response<T>
                    {
                        Ok = false,
                        Error = JsonSerializer.Deserialize<ProblemDetails>(content) ?? new ProblemDetails
                        {
                            Title = "Error",
                            Detail = "The request failed.",
                        }
                    };
                }
                else
                {
                    response = new Response<T>
                    {
                        Ok = true,
                        Data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })
                    };
                }

            }
            catch (Exception ex)
            {
                response = new Response<T>
                {
                    Ok = false,
                    Error = new ProblemDetails
                    {
                        Title = "Exception",
                        Detail = ex.Message
                    }
                };
            }
            finally
            {
                httpClient.Dispose();
            }

            return response!;
        }

        public void Authenticate(string accessToken, string refreshToken)
        {
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentException("Access token and refresh token cannot be null or empty.");
            }

            _accessToken = accessToken;
            _refreshToken = refreshToken;
        }

        public TokensDto GetTokens()
        {
            if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_refreshToken))
            {
                throw new InvalidOperationException("Access token and refresh token must be set before calling GetTokens.");
            }

            return new TokensDto
            {
                AccessToken = _accessToken,
                RefreshToken = _refreshToken
            };
        }

        public bool VerifyTokenRotation(string currentAccessToken, string currentRefreshToken)
        {
            if (string.IsNullOrEmpty(currentAccessToken) || string.IsNullOrEmpty(currentRefreshToken))
            {
                throw new ArgumentException("Access token and refresh token cannot be null or empty.");
            }

            return _accessToken != currentAccessToken || _refreshToken != currentRefreshToken;
        }
    }
}
