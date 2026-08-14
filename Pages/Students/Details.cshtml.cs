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
    public class DetailsModel : PageModel
    {
        private readonly RegistroDeEstudantes.Data.ApplicationDbContext _context;

        public DetailsModel(RegistroDeEstudantes.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
