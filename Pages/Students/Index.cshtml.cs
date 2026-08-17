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
    public class IndexModel : PageModel
    {
        private readonly RegistroDeEstudantes.Data.ApplicationDbContext _context;

        public IndexModel(RegistroDeEstudantes.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Student> Students { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Students = await _context.Students.ToListAsync();
        }
    }
}