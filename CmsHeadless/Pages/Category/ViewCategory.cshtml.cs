using System.Windows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using CmsHeadless.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CmsHeadless.Pages.Category
{

    [Authorize]
    public class ViewCategoryModel : PageModel
    {
        public const int numberPage = 5;
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public static int lastDelete = 0;
        public Models.Category CategoryNew { get; set; }

        public List<Models.Category> CategoryAvailable { get; set; }

        public ViewCategoryModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            CategoryAvailable = new List<Models.Category>();
        }


        public CategoryList<Models.Category> CategoryList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex) {

            IQueryable<Models.Category> selectCategoryQuery = from Category in _context.Category select Category;
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            CategoryList = await CategoryList<Models.Category>.CreateAsync(
                selectCategoryQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? categoryId) {
            lastDelete = 0;
            if (categoryId == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(categoryId);

            IQueryable<Models.Category> selectCategoryQuery;
            if (category == null)
            {
                return NotFound();
            }
            _context.Category.Remove(category);
            lastDelete=await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }
            selectCategoryQuery = from Category in _context.Category select Category;
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            CategoryList = await CategoryList<Models.Category>.CreateAsync(selectCategoryQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./ViewCategory");
        }

    }
}
