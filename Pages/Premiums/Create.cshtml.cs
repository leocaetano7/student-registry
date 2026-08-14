using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegistry.Data;
using StudentRegistry.Models;

namespace StudentRegistry.Pages_Premiums
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public Premium Premium { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Bug corrigido: ao reexibir a página por erro de validação, o
                // <select> de estudante ficava vazio porque ViewData["StudentId"]
                // só era preenchido em OnGet(), nunca em OnPostAsync().
                ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Email", Premium.StudentId);
                return Page();
            }

            _context.Premiums.Add(Premium);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
