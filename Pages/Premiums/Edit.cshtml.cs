using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RegistroDeEstudantes.Data;
using RegistroDeEstudantes.Models;

namespace RegistroDeEstudantes.Pages_Premiums
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<EditModel> _logger;

        public EditModel(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer, ILogger<EditModel> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
        }

        public Premium Premium { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var premium = await _context.Premiums
                .FirstOrDefaultAsync(p => p.Id == id);

            if (premium == null)
            {
                return NotFound();
            }

            Premium = premium;

            LoadStudentsSelectList(Premium.StudentId); 

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var premiumToUpdate = await _context.Premiums.FindAsync(id);

            if (premiumToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(
                premiumToUpdate,
                "Premium",
                p => p.Title,
                p => p.StartDate,
                p => p.EndDate,
                p => p.StudentId))
            {
                try
                {
                    await _context.SaveChangesAsync();

                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PremiumExists(id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar premium {PremiumId}", id);
                    ModelState.AddModelError(string.Empty, _localizer["PremiumSaveError"]);
                }
            }

            Premium = premiumToUpdate;

            LoadStudentsSelectList(Premium.StudentId); 
            return Page();
        }

        private bool PremiumExists(int id)
        {
            return _context.Premiums.Any(e => e.Id == id);
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