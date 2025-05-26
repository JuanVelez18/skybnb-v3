using application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentations.Interfaces;
using web_presentation.Core;

namespace web_presentation.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthPresentation _authPresentation;

        public LoginModel(IAuthPresentation authPresentation)
        {
            _authPresentation = authPresentation;
        }

        [BindProperty]
        public UserCredentialsDto Credentials { get; set; } = new UserCredentialsDto();

        private void SetTokensInCookies(TokensDto tokens)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Path = "/",
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Append("AccessToken", tokens.AccessToken, options);
            Response.Cookies.Append("RefreshToken", tokens.RefreshToken, options);
        }
        public IActionResult OnGet()
        {
            var hasActiveSession = Request.Cookies.ContainsKey("AccessToken") ||
                              Request.Cookies.ContainsKey("RefreshToken");

            if (hasActiveSession) return RedirectToPage(Routes.Home);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var tokens = await _authPresentation.LoginAsync(Credentials);
            SetTokensInCookies(tokens);

            return RedirectToPage(Routes.Home);
        }
    }
}
