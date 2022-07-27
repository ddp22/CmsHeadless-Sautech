using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsHeadless.Models;
using CmsHeadless.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using CmsHeadless.Pages.Category;

namespace CmsHeadless.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public List<Region> RegionAvailable;
        public List<Province> ProvinceAvailable;
        public ContentController(ILogger<ContentController> logger, CmsHeadlessDbContext contextDb)
        {
            _logger = logger;
            _contextDb = contextDb;
            IQueryable<Region> selectRegionQuery = from Region in _contextDb.Region select Region;
            RegionAvailable = selectRegionQuery.ToList<Region>();

            IQueryable<Models.Province> selectProvinceQuery = from Province in _contextDb.Province select Province;
            ProvinceAvailable = selectProvinceQuery.ToList<Models.Province>();
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult LoadRegion(int id)
        {
            List<Region> region = RegionAvailable.Where(c => c.NationId == id).ToList();
            return Json(region);
        }

        public JsonResult LoadProvince(int id)
        {
            List<Province> province = ProvinceAvailable.Where(c => c.RegionId == id).ToList();
            return Json(province);
        }

    }
}
