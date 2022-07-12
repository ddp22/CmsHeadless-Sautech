using System.Windows;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CmsHeadless.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CmsHeadless.ViewModels.Category;

namespace CmsHeadless.Pages.Category
{

    [Authorize]
    public class IndexModel : PageModel
    {
        public const int numberPage = 5;
        private readonly CmsHeadlessDbContext _context;
        private readonly IConfiguration Configuration;
        public static int lastCreate = 0;
        public static int lastDelete = 0;
        public static bool callDelete = false;
        public string pathName = "/img/category/";
        [BindProperty]
        public CategoryViewModel _formCategoryModel { get; set; }

        public static string searchString { get; set; }
        public Models.Category CategoryNew { get; set; }

        public List<Models.Category> CategoryAvailable { get; set; }

        public IndexModel(CmsHeadlessDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            CategoryAvailable = new List<Models.Category>();
            _formCategoryModel=new CategoryViewModel();
        }


        public CategoryList<Models.Category> CategoryList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex, string? searchString) {

            IQueryable<Models.Category> selectCategoryQuery;
            IQueryable<Models.Category> selectCategoryQueryOrder;

            selectCategoryQueryOrder = from Category in _context.Category select Category;
            selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
            if (!string.IsNullOrEmpty(searchString))
            {
                IndexModel.searchString = searchString;
                selectCategoryQuery = selectCategoryQuery.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString));
            }
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



        public async Task<IActionResult> OnPostCreateAsync(int? pageIndex, string? searchString)
        {
            lastCreate = 0;
            lastDelete = 0;
            IQueryable<Models.Category> selectCategoryQuery;
            IQueryable<Models.Category> selectCategoryQueryOrder;

            var pageSize = 5;

            int is_exsist  = _context.Category.Where(c => c.Name == _formCategoryModel.Name).Count();

            if (is_exsist>0) {
                ModelState.AddModelError("Make", "Non è stato possibile inserire la categoria perchè già esiste");
                selectCategoryQueryOrder = from Category in _context.Category select Category;
                selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
                if (!string.IsNullOrEmpty(searchString))
                {
                    selectCategoryQuery = selectCategoryQuery.Where(s => s.Name.Contains(searchString));
                }
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
                temp.Media = pathName + uniqueFileName;
            }
            if (DateTime.Now.Date> _formCategoryModel.CreationDate.Date)
            {
                ModelState.AddModelError("Make", "Inserire una data successiva a quella odierna");


                selectCategoryQueryOrder = from Category in _context.Category select Category;
                selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
                CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
                if (pageIndex == null)
                {
                    pageIndex = 1;
                }
                pageSize = Configuration.GetValue("PageSize", numberPage);
                CategoryList = await CategoryList<Models.Category>.CreateAsync(selectCategoryQuery.AsNoTracking(), pageIndex ?? 1, pageSize);


                return Page();
            }
            temp.CreationDate = _formCategoryModel.CreationDate;

        
        
            var entry = _context.Add(new Models.Category());
            entry.CurrentValues.SetValues(temp);
            lastCreate = await _context.SaveChangesAsync();
            if(lastCreate <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'inserimento");
                return Page();
            }

            selectCategoryQueryOrder = from Category in _context.Category select Category;
            selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
            if (pageIndex == null){
                pageIndex = 1;
            }
            pageSize = Configuration.GetValue("PageSize", numberPage);
            CategoryList = await CategoryList<Models.Category>.CreateAsync(selectCategoryQuery.AsNoTracking(), pageIndex ?? 1, pageSize);

            return RedirectToPage("./Index");
        }



        public async Task<IActionResult> OnGetDeleteAsync(int? pageIndex, int? categoryId) {
            lastDelete = 0;
            lastCreate = 0;
            callDelete = true;
            if (categoryId == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(categoryId);

            IQueryable<Models.Category> selectCategoryQueryOrder;
            IQueryable<Models.Category> selectCategoryQuery;
            if (category == null)
            {
                return NotFound();
            }

            string strPhysicalFolder = "wwwroot";
            FileInfo file = new FileInfo(strPhysicalFolder + category.Media);
            if (file.Exists)
            {
                file.Delete();
            }

            _context.Category.Remove(category);
            lastDelete=await _context.SaveChangesAsync();
            if (lastDelete <= 0)
            {
                ModelState.AddModelError("Make", "Errore nell'eliminazione");
                return Page();
            }
            selectCategoryQueryOrder = from Category in _context.Category select Category;
            selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            var pageSize = Configuration.GetValue("PageSize", numberPage);
            CategoryList = await CategoryList<Models.Category>.CreateAsync(selectCategoryQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            return RedirectToPage("./Index");
        }

    }
}
