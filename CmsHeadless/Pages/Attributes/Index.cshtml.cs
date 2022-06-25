using CmsHeadless.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;

namespace CmsHeadless.Pages.Attributes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CmsHeadlessDbContext _context;

        [BindProperty]
        public Models.Attributes AttributesNew { get; set; }

        public IndexModel(CmsHeadlessDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entry = _context.Add(new Models.Attributes());
            entry.CurrentValues.SetValues(AttributesNew);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        public void OnGet()
        {
        }
    }
}
