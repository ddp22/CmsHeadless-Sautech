using System.Windows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using CmsHeadless.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Tag
{

    [Authorize]
    public class ViewTagModel : PageModel
    {
        public const int numberPage = 5;
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public static int lastDelete = 0;
        public static string searchString { get; set; }
        public Models.Tag TagNew { get; set; }

        public List<Models.Tag> tagAvailable { get; set; }

        public ViewTagModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            tagAvailable = new List<Models.Tag>();
        }


        public TagList<Models.Tag> TagList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex, string? searchString) {

            IQueryable<Models.Tag> selectTagQuery;
            IQueryable<Models.Tag> selectTagQueryOrder;
            selectTagQueryOrder = from Tag in _context.Tag select Tag;
            selectTagQuery=selectTagQueryOrder.OrderByDescending(x => x.TagId);
            if (!string.IsNullOrEmpty(searchString))
            {
                ViewTagModel.searchString = searchString;
                selectTagQuery = selectTagQuery.Where(s => s.Name.Contains(searchString));
            }
            tagAvailable = selectTagQuery.ToList<Models.Tag>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            TagList = await TagList<Models.Tag>.CreateAsync(
                selectTagQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? tagId) {
            lastDelete = 0;
            if (tagId == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag.FindAsync(tagId);

            IQueryable<Models.Tag> selectTagQuery;
            IQueryable<Models.Tag> selectTagQueryOrder;
            if (tag == null)
            {
                return NotFound();
            }
            _context.Tag.Remove(tag);
            lastDelete=await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }
            selectTagQueryOrder = from Tag in _context.Tag select Tag;
            selectTagQuery = selectTagQueryOrder.OrderByDescending(x => x.TagId);
            tagAvailable = selectTagQuery.ToList<Models.Tag>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            TagList = await TagList<Models.Tag>.CreateAsync(selectTagQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./ViewTag");
        }

    }
}
