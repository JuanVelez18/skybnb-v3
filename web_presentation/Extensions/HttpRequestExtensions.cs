using application.DTOs;
using Microsoft.AspNetCore.Http;

namespace web_presentation.Extensions
{
    /// <summary>
    /// Extension methods for HttpRequest to handle authentication tokens from cookies
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Checks if the request has active authentication tokens in cookies
        /// </summary>
        /// <param name="request">The HTTP request to check</param>
        /// <returns>True if either AccessToken or RefreshToken is present in cookies</returns>
        public static bool HasActiveSession(this HttpRequest request)
        {
            return request.Cookies.ContainsKey("AccessToken") ||
                   request.Cookies.ContainsKey("RefreshToken");
        }

        /// <summary>
        /// Gets authentication tokens from cookies
        /// </summary>
        /// <param name="request">The HTTP request to get cookies from</param>
        /// <returns>TokensDto if both tokens are present, null otherwise</returns>
        public static TokensDto? GetAuthTokensFromCookies(this HttpRequest request)
        {
            var accessToken = request.Cookies["AccessToken"];
            var refreshToken = request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return null;

            return new TokensDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        /// <summary>
        /// Gets the access token from cookies
        /// </summary>
        /// <param name="request">The HTTP request to get cookie from</param>
        /// <returns>Access token string if present, null otherwise</returns>
        public static string? GetAccessTokenFromCookies(this HttpRequest request)
        {
            return request.Cookies["AccessToken"];
        }

        /// <summary>
        /// Gets the refresh token from cookies
        /// </summary>
        /// <param name="request">The HTTP request to get cookie from</param>
        /// <returns>Refresh token string if present, null otherwise</returns>
        public static string? GetRefreshTokenFromCookies(this HttpRequest request)
        {
            return request.Cookies["RefreshToken"];
        }
    }
}
