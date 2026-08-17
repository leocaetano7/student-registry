using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RegistroDeEstudantes.Data;
using RegistroDeEstudantes.Models;

namespace RegistroDeEstudantes.Pages_Students
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<DeleteModel> _logger; // <<< NOVO

        public DeleteModel(
            ApplicationDbContext context,
            IStringLocalizer<SharedResource> localizer,
            ILogger<DeleteModel> logger) // <<< NOVO parâmetro
        {
            _context = context;
            _localizer = localizer;
            _logger = logger; // <<< NOVO
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            Student = student;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return RedirectToPage("./Index");
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) // <<< adicionou "ex"
            {
                _logger.LogError(ex, "Erro ao excluir estudante {StudentId}", student.Id); // <<< NOVO
                Student = student;

                ModelState.AddModelError(
                    string.Empty,
                    _localizer["StudentDeleteWithPremiumError"]
                );

                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}