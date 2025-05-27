using application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentations.Interfaces;
using web_presentation.Core;
using web_presentation.Extensions;

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

        public IActionResult OnGet()
        {
            if (Request.HasActiveSession())
                return RedirectToPage(Routes.Home);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var tokens = await _authPresentation.LoginAsync(Credentials);
            Response.SetAuthTokenCookies(tokens);

            TempData["ShouldPassCookiesToSPA"] = true;
            return RedirectToPage(Routes.Home);
        }
    }
}
