using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CmsHeadless.Models;

namespace CmsHeadlessApi.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public CategoryController(ILogger<CategoryController> logger, CmsHeadlessDbContext contextDb)
        {
            _logger = logger;
            _contextDb = contextDb;
        }

        [HttpGet]
        public JsonResult GetAllCategory(int? idCategory, string? nameCategory)
        {
            if (idCategory ==null && nameCategory == null) {
                return Json(_contextDb.Category.ToList<Category>());
            }
            else if(nameCategory == null){
                var categoryItem = _contextDb.Category.FindAsync(idCategory);
                if (categoryItem == null)
                {
                    return Json(null);
                }
                else
                {
                    return Json(categoryItem.Result);
                }
            }
            else if(idCategory == null)
            {
                var categoryItem = _contextDb.Category.Where(c=>c.Name.Contains(nameCategory)).ToList<Category>();
                if (categoryItem.Count()==0)
                {
                    return Json(null);
                }
                else
                {
                    return Json(categoryItem);
                }
            }
            else
            {
                var categoryItem = _contextDb.Category.Where(c => c.CategoryId == idCategory && c.Name.Contains(nameCategory)).ToList<Category>();
                if (categoryItem.Count() == 0)
                {
                    return Json(null);
                }
                else
                {
                    return Json(categoryItem);
                }
            }
        }

        
    }
}