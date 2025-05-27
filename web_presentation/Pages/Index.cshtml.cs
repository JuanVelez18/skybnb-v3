using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace web_presentation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public bool ShouldPassCookiesToSPA { get; set; } = false;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            TempData.TryGetValue("ShouldPassCookiesToSPA", out var passTokensCookies);
            if (passTokensCookies is bool shouldPass && shouldPass)
            {
                ShouldPassCookiesToSPA = true;
            }
        }
    }
}
