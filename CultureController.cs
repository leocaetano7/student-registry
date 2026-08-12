using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace testeleo.Controllers;

[Route("[controller]/[action]")]
public class CultureController : Controller
{
    // Mantido em sincronia com os idiomas configurados em Program.cs.
    // Bug corrigido: antes, qualquer valor de "culture" era aceito e gravado
    // diretamente no cookie, sem validação contra a lista de idiomas suportados.
    private static readonly string[] SupportedCultures = { "pt-BR", "en-US" };

    [HttpPost]
    public IActionResult SetLanguage(string culture, string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(culture) || Array.IndexOf(SupportedCultures, culture) < 0)
        {
            culture = SupportedCultures[0];
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
        );

    
        return LocalRedirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);
    }
}