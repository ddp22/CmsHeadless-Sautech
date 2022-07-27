using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsHeadless.Models;
using CmsHeadless.ViewModels;
/*using System;
using System.IO;
using System.Threading.Tasks;
using CmsHeadless.Pages.Category;

namespace CmsHeadless.Controllers
{
    public class CategoryController : Controller
    {
    private readonly CmsHeadlessDbContext dbContext;
    public CategoryController(CmsHeadlessDbContext context)
    {
        dbContext = context;
    }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var categories = from Category in dbContext.Category
                           select Category;
            
            int pageSize = 3;
            return View(await CategoryList<Category>.CreateAsync(categories.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult New()
        {
            return View();
        }

        



    }
}

*/