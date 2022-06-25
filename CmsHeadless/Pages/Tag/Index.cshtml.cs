using CmsHeadless.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;

namespace CmsHeadless.Pages.Tag
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CmsHeadlessDbContext _context;

        [BindProperty]
        public Models.Tag TagNew { get; set; }

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

            var entry = _context.Add(new Models.Tag());
            entry.CurrentValues.SetValues(TagNew);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        public void OnGet()
        {
        }
    }
}
