using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using application.DTOs;

namespace presentations
{
    public class Comunication
    {
        private string _host = string.Empty;
        private string? _accessToken;
        private string? _refreshToken;

        public Comunication(string host)
        {
            _host = host;
        }

        public class Response<T>
        {
            public bool Ok { get; init; }
            public string Message { get; init; } = string.Empty;
            public T? Data { get; init; }
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

                if (!message.IsSuccessStatusCode)
                {
                    response = new Response<T>
                    {
                        Ok = false,
                        Message = message.ReasonPhrase ?? "Unknown error"
                    };
                }
                else
                {
                    response = new Response<T>
                    {
                        Ok = true,
                        Message = message.ReasonPhrase ?? "Success",
                        Data = JsonSerializer.Deserialize<T>(content)
                    };
                }

            }
            catch (Exception ex)
            {
                response = new Response<T>
                {
                    Ok = false,
                    Message = ex.Message
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
