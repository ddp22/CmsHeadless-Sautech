using CmsHeadless.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;


namespace CmsHeadless.Pages.Category
{

    [Authorize]
    public class IndexModel : PageModel
    {
        //private readonly Models.CmsHeadlessDbContext _context;

        private readonly CmsHeadlessDbContext _context;

        [BindProperty]
        public Models.Category CategoryNew { get; set; }

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
            
            var entry = _context.Add(new Models.Category());
            entry.CurrentValues.SetValues(CategoryNew);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }


    }
}
