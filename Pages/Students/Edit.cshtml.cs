using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RegistroDeEstudantes.Data;
using RegistroDeEstudantes.Models;

namespace RegistroDeEstudantes.Pages_Students
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<EditModel> _logger; // <<< NOVO

        public EditModel(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer, ILogger<EditModel> logger) // <<< NOVO parâmetro
        {
            _context = context;
            _localizer = localizer;
            _logger = logger; // <<< NOVO
        }
        
        public Student Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            Student = student;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var studentToUpdate = await _context.Students.FindAsync(id);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(
                    studentToUpdate,
                    "Student",
                    s => s.Name,
                    s => s.Email))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateException ex) // <<< adicionou "ex"
                {
                    _logger.LogError(ex, "Erro ao atualizar estudante {StudentId}", studentToUpdate.Id); // <<< NOVO
                    ModelState.AddModelError(
                        string.Empty,
                        _localizer["StudentUpdateError"]
                    );
                }
            }

            Student = studentToUpdate;
            
            return Page();
        }
    }
}