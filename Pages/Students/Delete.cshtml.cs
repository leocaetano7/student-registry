using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RegistroDeEstudantes.Data;
using RegistroDeEstudantes.Models;

namespace RegistroDeEstudantes.Pages_Students
{
    public class DeleteModel : PageModel
    {
        private readonly RegistroDeEstudantes.Data.ApplicationDbContext _context;

        public DeleteModel(RegistroDeEstudantes.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);

            if (student is not null)
            {
                Student = student;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                try
                {
                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Não é possível excluir este aluno porque ele possui planos Premium ativos.");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        } 
    } 
} 

