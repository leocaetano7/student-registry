using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegistry.Data;
using StudentRegistry.Models;

namespace StudentRegistry.Pages_Students
{
    public class CreateModel : PageModel
    {
        private readonly StudentRegistry.Data.ApplicationDbContext _context;

        public CreateModel(StudentRegistry.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                _context.Students.Add(Student);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar o estudante. Verifique se os dados (como e-mail) já estão cadastrados.");
                return Page();
            } 
        } 
    } 
} 
        


