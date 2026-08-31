using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using StudentRegistry.Data;
using StudentRegistry.Models;

namespace StudentRegistry.Pages_Premiums
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer, ILogger<CreateModel> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            LoadStudentsSelectList();
            return Page();
        }

        [BindProperty]
        public Premium Premium { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadStudentsSelectList(Premium.StudentId);
                return Page();
            }

            try
            {
                _context.Premiums.Add(Premium);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar premium para o estudante {StudentId}", Premium.StudentId);
                ModelState.AddModelError(string.Empty, _localizer["PremiumSaveError"]);

                LoadStudentsSelectList(Premium.StudentId);
                return Page();
            }
        }

        private void LoadStudentsSelectList(int? selectedId = null)
        {
            ViewData["StudentId"] = new SelectList(
                _context.Students,
                "Id",
                "Email",
                selectedId
            );
        }
    }
}
