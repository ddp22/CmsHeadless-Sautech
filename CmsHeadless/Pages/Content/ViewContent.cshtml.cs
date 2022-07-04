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
        public static string searchString { get; set; }
        public Models.Content ContentNew { get; set; }

        public List<Models.Content> ContentAvailable { get; set; }

        public ViewContentModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            ContentAvailable = new List<Models.Content>();
        }


        public ContentList<Models.Content> ContentList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex, string? searchString) {
            IQueryable<Models.Content> selectContentQueryOrder;
            IQueryable<Models.Content> selectContentQuery;

            selectContentQueryOrder = from Content in _context.Content select Content;
            selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
            if (!string.IsNullOrEmpty(searchString))
            {
                ViewContentModel.searchString = searchString;
                List<int> tempContentAttribute = (from c in _context.Content
                                                  join ca in _context.ContentAttributes
                                                  on c.ContentId equals ca.ContentId
                                                  join a in _context.Attributes
                                                  on ca.AttributesId equals a.AttributesId
                                                  where a.AttributeName.Contains(searchString)
                                                  select c.ContentId).ToList<int>();

                List<int> tempContentTag = (from c in _context.Content
                                            join ct in _context.ContentTag
                                            on c.ContentId equals ct.ContentId
                                            join t in _context.Tag
                                            on ct.TagId equals t.TagId
                                            where t.Name.Contains(searchString)
                                            select c.ContentId).ToList<int>();

                List<int> tempContentCategory = (from c in _context.Content
                                                 join cc in _context.ContentCategory
                                                 on c.ContentId equals cc.ContentId
                                                 join ca in _context.Category
                                                 on cc.CategoryId equals ca.CategoryId
                                                 where ca.Name.Contains(searchString)
                                                 select c.ContentId).ToList<int>();

                selectContentQuery = selectContentQuery.Where(s => (s.Title.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Text.Contains(searchString))
                || tempContentAttribute.Contains(s.ContentId)
                || tempContentTag.Contains(s.ContentId)
                || tempContentCategory.Contains(s.ContentId));
            }
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
            IQueryable<Models.Content> selectContentQueryOrder;
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
            selectContentQueryOrder = from Content in _context.Content select Content;
            selectContentQuery = selectContentQueryOrder.OrderByDescending(c => c.ContentId);
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
