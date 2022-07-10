using CmsHeadless.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using CmsHeadless.ViewModels.Tag;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Tag
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public const int numberPage = 5;
        public static int lastCreate = 0;
        public static int lastDelete = 0;
        public static bool callDelete = false;
        public static string searchString { get; set; }

        [BindProperty]
        public TagViewModel _formTagModel { get; set; }
        public Models.Tag TagNew { get; set; }
        public List<Models.Tag> TagAvailable { get; set; }

        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            TagAvailable= new List<Models.Tag>();
        }

        public TagList<Models.Tag> TagList { get; set; }


        public async Task<IActionResult> OnGetAsync(int? pageIndex, string? searchString)
        {
            IQueryable<Models.Tag> selectTagQuery;
            IQueryable<Models.Tag> selectTagQueryOrder;

            selectTagQueryOrder = from Tag in _context.Tag select Tag;
            selectTagQuery = selectTagQueryOrder.OrderByDescending(x => x.TagId);
            if (!string.IsNullOrEmpty(searchString))
            {
                IndexModel.searchString=searchString;
                selectTagQuery = selectTagQuery.Where(s => s.Name.Contains(searchString));
            }
            TagAvailable = selectTagQuery.ToList<Models.Tag>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            TagList = await TagList<Models.Tag>.CreateAsync(
                selectTagQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(int? pageIndex)
        {
            lastCreate = 0;
            lastDelete = 0;
            IQueryable<Models.Tag> selectTagQuery;
            IQueryable<Models.Tag> selectTagQueryOrder;

            var pageSize = 5;

            int is_exsist = _context.Tag.Where(c => c.Name == _formTagModel.Name).Count();

            if (is_exsist > 0)
            {
                ModelState.AddModelError("Make", "Non è stato possibile inserire il tag perchè già esiste");
                selectTagQueryOrder = from Tag in _context.Tag select Tag;
                selectTagQuery = selectTagQueryOrder.OrderByDescending(x => x.TagId);
                TagAvailable = selectTagQuery.ToList<Models.Tag>();
                if (pageIndex == null)
                {
                    pageIndex = 1;
                }
                pageSize = Configuration.GetValue("PageSize", numberPage);
                TagList = await TagList<Models.Tag>.CreateAsync(selectTagQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
                return Page();
            }

            Models.Tag temp = new Models.Tag();
            temp.Name = _formTagModel.Name;
            temp.Url = _formTagModel.Url;
            var entry = _context.Add(new Models.Tag());
            entry.CurrentValues.SetValues(temp);
            lastCreate = await _context.SaveChangesAsync();
            if (lastCreate <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }

            selectTagQueryOrder = from Tag in _context.Tag select Tag;
            selectTagQuery = selectTagQueryOrder.OrderByDescending(x => x.TagId);
            TagAvailable = selectTagQuery.ToList<Models.Tag>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            pageSize = Configuration.GetValue("PageSize", numberPage);
            TagList = await TagList<Models.Tag>.CreateAsync(selectTagQuery.AsNoTracking(), pageIndex ?? 1, pageSize);

            return RedirectToPage("./Index");
        }


        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? tagId)
        {
            lastDelete = 0;
            lastCreate = 0;
            callDelete = true;
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
            lastDelete = await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'eliminazione");
                return Page();
            }
            selectTagQueryOrder = from Tag in _context.Tag select Tag;
            selectTagQuery = selectTagQueryOrder.OrderByDescending(x => x.TagId);
            TagAvailable = selectTagQuery.ToList<Models.Tag>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            TagList = await TagList<Models.Tag>.CreateAsync(selectTagQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./Index");
        }

    }
}
