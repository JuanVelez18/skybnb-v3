using application.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace web_presentation.Extensions
{
    /// <summary>
    /// Extension methods for PageModel to handle authentication tokens
    /// </summary>
    public static class PageModelExtensions
    {
        /// <summary>
        /// Sets authentication tokens in cookies for the current page model
        /// </summary>
        /// <param name="pageModel">The page model</param>
        /// <param name="tokens">The tokens to set</param>
        public static void SetAuthTokens(this PageModel pageModel, TokensDto tokens)
        {
            pageModel.Response.SetAuthTokenCookies(tokens);
        }

        /// <summary>
        /// Clears authentication tokens from cookies for the current page model
        /// </summary>
        /// <param name="pageModel">The page model</param>
        public static void ClearAuthTokens(this PageModel pageModel)
        {
            pageModel.Response.ClearAuthTokenCookies();
        }

        /// <summary>
        /// Checks if the current page model has an active session
        /// </summary>
        /// <param name="pageModel">The page model</param>
        /// <returns>True if there's an active session</returns>
        public static bool HasActiveSession(this PageModel pageModel)
        {
            return pageModel.Request.HasActiveSession();
        }

        /// <summary>
        /// Gets authentication tokens from cookies for the current page model
        /// </summary>
        /// <param name="pageModel">The page model</param>
        /// <returns>TokensDto if both tokens are present, null otherwise</returns>
        public static TokensDto? GetAuthTokens(this PageModel pageModel)
        {
            return pageModel.Request.GetAuthTokensFromCookies();
        }
    }
}
