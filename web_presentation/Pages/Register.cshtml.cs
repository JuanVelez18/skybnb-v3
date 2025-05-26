using application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using presentations.Interfaces;
using web_presentation.Core;
using web_presentation.Extensions;

namespace web_presentation.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthPresentation _authPresentation;
        private readonly ICountryPresentation _countryPresentation;
        private readonly ICitytPresentation _cityPresentation;

        public RegisterModel(
            IAuthPresentation authPresentation,
            ICountryPresentation countryPresentation,
            ICitytPresentation cityPresentation
        )
        {
            _authPresentation = authPresentation;
            _countryPresentation = countryPresentation;
            _cityPresentation = cityPresentation;
        }

        [BindProperty]
        public UserCreationDto UserCreation { get; set; } = new();

        [BindProperty]
        public bool LikeGuest { get; set; } = false;

        [BindProperty]
        public AddressDto Address { get; set; } = new();

        public List<CountryListDto> Countries { get; set; } = [];
        public List<CityListDto> Cities { get; set; } = [];

        private async Task LoadCountriesAsync()
        {
            Countries = await _countryPresentation.GetAllAsync();
        }
        private async Task LoadCitiesAsync()
        {
            Cities = await _cityPresentation.GetAllAsync();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.HasActiveSession())
            {
                return RedirectToPage(Routes.Home);
            }

            await LoadCountriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!LikeGuest)
            {
                var keysToRemove = new List<string>
                {
                    nameof(Address.Street),
                    nameof(Address.StreetNumber),
                    nameof(Address.IntersectionNumber),
                    nameof(Address.DoorNumber),
                    nameof(Address.CityId),
                    nameof(Address.Complement),
                    nameof(Address.Latitude),
                    nameof(Address.Longitude)
                };

                foreach (var key in keysToRemove)
                {
                    ModelState.Remove(key);
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadCountriesAsync();
                await LoadCitiesAsync();
                return Page();
            }

            TokensDto tokens;

            if (LikeGuest)
            {
                tokens = await _authPresentation.RegisterGuestAsync(new GuestCreationDto
                {
                    Dni = UserCreation.Dni,
                    FirstName = UserCreation.FirstName,
                    LastName = UserCreation.LastName,
                    Email = UserCreation.Email,
                    Password = UserCreation.Password,
                    Birthday = UserCreation.Birthday,
                    CountryId = UserCreation.CountryId,
                    Phone = UserCreation.Phone,
                    Address = Address
                });
            }
            else
            {
                tokens = await _authPresentation.RegisterHostAsync(UserCreation);
            }

            Response.SetAuthTokenCookies(tokens);

            TempData["ShouldPassCookiesToSPA"] = true;
            return RedirectToPage(Routes.Home, new { ShouldPassCookiesToSPA = true });
        }
    }
}
