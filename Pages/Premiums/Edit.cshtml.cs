using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testeleo.Data;
using testeleo.Models;

namespace testeleo.Pages_Premiums
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Premium Premium { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var premium = await _context.Premiums.FirstOrDefaultAsync(m => m.Id == id);
            if (premium == null)
            {
                return NotFound();
            }
            Premium = premium;
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Email", Premium.StudentId);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Mesmo bug de Create.cshtml.cs: o <select> de estudante ficava
                // vazio ao reexibir o formulário por erro de validação.
                ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Email", Premium.StudentId);
                return Page();
            }

            _context.Attach(Premium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PremiumExists(Premium.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PremiumExists(int id)
        {
            return _context.Premiums.Any(e => e.Id == id);
        }
    }
}
