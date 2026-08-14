using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;

namespace RegistroDeEstudantes.Pages;

public class IndexModel : PageModel
{
    private static readonly string[] SupportedCultures = { "pt-BR", "en-US" };

    public void OnGet()
    {
    }

    public IActionResult OnPostSetLanguage(string culture, string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(culture) ||
            Array.IndexOf(SupportedCultures, culture) < 0)
        {
            culture = SupportedCultures[0];
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(culture)
            ),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                Path = "/",
                IsEssential = true
            }
        );

        return LocalRedirect(
            string.IsNullOrWhiteSpace(returnUrl) ? "~/" : returnUrl
        );
    }
}