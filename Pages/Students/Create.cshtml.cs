using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using StudentRegistry.Data;
using StudentRegistry.Models;
using Microsoft.Extensions.Logging;

namespace StudentRegistry.Pages_Students
{
    public class CreateModel : PageModel
    {
        private readonly StudentRegistry.Data.ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(
            StudentRegistry.Data.ApplicationDbContext context,
            IStringLocalizer<SharedResource> localizer,
            ILogger<CreateModel> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
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
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar estudante com e-mail {Email}", Student.Email);
                ModelState.AddModelError(string.Empty, _localizer["StudentSaveError"]);
                return Page();
            }
        
        } 
    } 
}
