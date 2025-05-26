using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_presentation.Core;
using web_presentation.Extensions;

namespace web_presentation.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            Response.ClearAuthTokenCookies();

            return RedirectToPage(Routes.Login);
        }
    }
}
