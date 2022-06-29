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
    public class IndexModel : PageModel
    {
        public const int numberPage = 5;
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        [BindProperty]
        public CategoryViewModel _formCategoryModel { get; set; }


        public Models.Category CategoryNew { get; set; }

        public List<Models.Category> CategoryAvailable { get; set; }

        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration)
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



        public async Task<IActionResult> OnPostAsync(int? pageIndex)
        {
            IQueryable<Models.Category> selectCategoryQuery;

            var pageSize = 10;

            int is_exsist  = _context.Category.Where(c => c.Name == _formCategoryModel.Name).Count();

            if (is_exsist>0) {
                ModelState.AddModelError("Make", "Utente già inserito");
                selectCategoryQuery = from Category in _context.Category select Category;
                CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
                if (pageIndex == null)
                {
                    pageIndex = 1;
                }
                pageSize = Configuration.GetValue("PageSize", numberPage);
                CategoryList = await CategoryList<Models.Category>.CreateAsync(selectCategoryQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
                return Page();
            }

            string uniqueFileName = null;
            if (_formCategoryModel.Media != null)
            {
                string uploadsFolder = Path.Combine("wwwroot/img/category");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + _formCategoryModel.Media.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    _formCategoryModel.Media.CopyTo(fileStream);
                }
            }


            Models.Category temp = new Models.Category();
            temp.Name = _formCategoryModel.Name;
            temp.Description = _formCategoryModel.Description;
            temp.CategoryParentId = _formCategoryModel.CategoryParentId;
            if (_formCategoryModel.Media != null)
            {
                temp.Media = uniqueFileName;
            }
            temp.CreationDate = _formCategoryModel.CreationDate;

        
        
            var entry = _context.Add(new Models.Category());
            entry.CurrentValues.SetValues(temp);
            await _context.SaveChangesAsync();


            selectCategoryQuery = from Category in _context.Category select Category;
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
            if (pageIndex == null){
                pageIndex = 1;
            }
            pageSize = Configuration.GetValue("PageSize", numberPage);
            CategoryList = await CategoryList<Models.Category>.CreateAsync(selectCategoryQuery.AsNoTracking(), pageIndex ?? 1, pageSize);

            return RedirectToPage("./Index");
        }

    }
}
