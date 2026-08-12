using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;

namespace testeleo.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    private static readonly string[] SupportedCultures = { "pt-BR", "en-US" };

    // Método que captura o clique do botão e salva o idioma de forma definitiva no Cookie.
    // Bug corrigido: "culture" não era validado contra a lista de idiomas suportados,
    // e "returnUrl" nulo/vazio causava exceção no LocalRedirect.
    public IActionResult OnPostSetLanguage(string culture, string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(culture) || Array.IndexOf(SupportedCultures, culture) < 0)
        {
            culture = SupportedCultures[0];
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), Path = "/", IsEssential = true }
        );

        return LocalRedirect(string.IsNullOrWhiteSpace(returnUrl) ? "~/" : returnUrl);
    }
}
