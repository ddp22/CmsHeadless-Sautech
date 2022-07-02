using System.Windows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using CmsHeadless.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Content
{

    [Authorize]
    public class ViewContentModel : PageModel
    {
        public const int numberPage = 5;
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public static int lastDelete = 0;
        public Models.Content ContentNew { get; set; }

        public List<Models.Content> ContentAvailable { get; set; }

        public ViewContentModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            ContentAvailable = new List<Models.Content>();
        }


        public ContentList<Models.Content> ContentList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex) {

            IQueryable<Models.Content> selectContentQuery = from Content in _context.Content select Content;
            ContentAvailable = selectContentQuery.ToList<Models.Content>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            ContentList = await ContentList<Models.Content>.CreateAsync(
                selectContentQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? contentId) {
            lastDelete = 0;
            if (contentId == null)
            {
                return NotFound();
            }

            var content = await _context.Content.FindAsync(contentId);

            IQueryable<Models.Content> selectContentQuery;
            if (content == null)
            {
                return NotFound();
            }
            _context.Content.Remove(content);
            lastDelete=await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }
            selectContentQuery = from Content in _context.Content select Content;
            ContentAvailable = selectContentQuery.ToList<Models.Content>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            ContentList = await ContentList<Models.Content>.CreateAsync(selectContentQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./ViewContent");
        }

    }
}
