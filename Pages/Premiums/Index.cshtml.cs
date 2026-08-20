using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentRegistry.Data;
using StudentRegistry.Models;

namespace StudentRegistry.Pages_Premiums
{
    public class IndexModel : PageModel
    {
        private readonly StudentRegistry.Data.ApplicationDbContext _context;

        public IndexModel(StudentRegistry.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Premium> Premiums { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Premiums = await _context.Premiums
                .Include(p => p.Student).ToListAsync();
        }
    }
}
