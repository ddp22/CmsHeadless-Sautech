using CmsHeadless.Models;
using CmsHeadless.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CmsHeadless.Pages.Category
{

    public class EditCategoryModel : PageModel
    {
        IQueryable<Models.Category> selectCategoryQueryOrder;
        IQueryable<Models.Category> selectCategoryQuery;
        public static int EditCategoryId=0;
        public static int lastEdit = 0;
        public Models.Category category;
        public Models.Category EditCategoryNew { get; set; }
        public string pathName = "/img/category/";
        [BindProperty]
        public EditCategoryViewModel _formEditCategoryModel { get; set; }
        private readonly CmsHeadlessDbContext _context;
        public List<Models.Category> CategoryAvailable { get; set; }
        public string CreationDate;

        public EditCategoryModel(CmsHeadlessDbContext context)
        {
            _context = context;
            CategoryAvailable= new List<Models.Category>();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            selectCategoryQueryOrder = from Category in _context.Category select Category;
            selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
            if (id == null)
            {
                if (EditCategoryId != 0)
                {
                    id = EditCategoryId;
                }
                else
                {
                    return NotFound();
                }
                
            }
            EditCategoryId = (int)id;
            if (_context==null || _context.Category == null)
            {
                return NotFound();
            }
            category  = await _context.Category.FindAsync(id);
            CreationDate =category.CreationDate.ToString("yyyy-MM-dd") ;
            if (category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int CategoryId)
        {
            lastEdit = 0;
            var categoryToUpdate = await _context.Category.FindAsync(CategoryId);

            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            if (categoryToUpdate.Name != _formEditCategoryModel.Name)
            {
                var categoryToSearch = from Category in _context.Category
                                       where Category.Name == _formEditCategoryModel.Name
                                       select Category;
                if (categoryToSearch.Count<Models.Category>() != 0)
                {
                    ModelState.AddModelError("Make", "Categoria già esistente. Inserirne un'altra");
                    selectCategoryQueryOrder = from Category in _context.Category select Category;
                    selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
                    CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
                    category = await _context.Category.FindAsync(CategoryId);

                    CreationDate = category.CreationDate.ToString("yyyy-MM-dd");
                    return Page();
                }
                
            }

            string uniqueFileName = null;
            if (_formEditCategoryModel.Media != null)
            {
                string strPhysicalFolder = "wwwroot";
                FileInfo file = new FileInfo(strPhysicalFolder + categoryToUpdate.Media);
                if (file.Exists)
                {
                    file.Delete();
                }
                string uploadsFolder = Path.Combine("wwwroot/img/category");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + _formEditCategoryModel.Media.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    _formEditCategoryModel.Media.CopyTo(fileStream);
                }
            }

            categoryToUpdate.Name = _formEditCategoryModel.Name;
            categoryToUpdate.Description = _formEditCategoryModel.Description;
            categoryToUpdate.CreationDate = _formEditCategoryModel.CreationDate;
            if (_formEditCategoryModel.Media != null)
            {
                categoryToUpdate.Media = pathName + uniqueFileName;
            }
            if (DateTime.Now.Date > _formEditCategoryModel.CreationDate.Date)
            {
                ModelState.AddModelError("Make", "Inserire una data successiva a quella odierna");
                selectCategoryQueryOrder = from Category in _context.Category select Category;
                selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
                CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
                category = await _context.Category.FindAsync(CategoryId);

                CreationDate = category.CreationDate.ToString("yyyy-MM-dd");
                return Page();
            }

            categoryToUpdate.CategoryParentId = _formEditCategoryModel.CategoryParentId;
            lastEdit = await _context.SaveChangesAsync();

            selectCategoryQueryOrder = from Category in _context.Category select Category;
            selectCategoryQuery = selectCategoryQueryOrder.OrderByDescending(x => x.CategoryId);
            CategoryAvailable = selectCategoryQuery.ToList<Models.Category>();
            category = await _context.Category.FindAsync(CategoryId);
            CreationDate = category.CreationDate.ToString("yyyy-MM-dd");
            return RedirectToPage("./EditCategory" , new { id=CategoryId });
        }
    }
}
