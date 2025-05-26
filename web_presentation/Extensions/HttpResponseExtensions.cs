using application.DTOs;
using Microsoft.AspNetCore.Http;

namespace web_presentation.Extensions
{
    /// <summary>
    /// Extension methods for HttpResponse to handle authentication tokens in cookies
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Sets authentication tokens (AccessToken and RefreshToken) as HTTP-only cookies
        /// </summary>
        /// <param name="response">The HTTP response to add cookies to</param>
        /// <param name="tokens">The tokens DTO containing AccessToken and RefreshToken</param>
        /// <param name="options">Optional custom cookie options. If null, default secure options will be used</param>
        public static void SetAuthTokenCookies(this HttpResponse response, TokensDto tokens, CookieOptions? options = null)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            var cookieOptions = options ?? new CookieOptions
            {
                HttpOnly = true,
                Path = "/",
                SameSite = SameSiteMode.Lax,
                Secure = false // Set to true in production with HTTPS
            };

            response.Cookies.Append("AccessToken", tokens.AccessToken, cookieOptions);
            response.Cookies.Append("RefreshToken", tokens.RefreshToken, cookieOptions);
        }

        /// <summary>
        /// Removes authentication tokens from cookies
        /// </summary>
        /// <param name="response">The HTTP response to remove cookies from</param>
        public static void ClearAuthTokenCookies(this HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Path = "/",
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(-1) // Expire the cookie
            };

            response.Cookies.Append("AccessToken", "", cookieOptions);
            response.Cookies.Append("RefreshToken", "", cookieOptions);
        }
    }
}
