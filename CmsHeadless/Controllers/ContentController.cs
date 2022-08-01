using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsHeadless.Models;
using CmsHeadless.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using CmsHeadless.Pages.Category;
using CmsHeadless.AuthenticationJWT;

namespace CmsHeadless.Controllers
{
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        private readonly ResponseApi _response;
        private string generatedToken = null;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        public List<Region> RegionAvailable;
        public List<Province> ProvinceAvailable;
        public ContentController(ILogger<ContentController> logger, CmsHeadlessDbContext contextDb, ResponseApi response, ITokenService tokenService, IConfiguration config)
        {
            _logger = logger;
            _contextDb = contextDb;
            _response = response;
            _tokenService = tokenService;
            _config = config;

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
            //string mail = "f.sarno01@gmail.com"; 
            //_response.User = (from User in _contextDb.CmsUser select User).Where(c => c.Email == mail).ToList().First();
            //var role = (from UserRoles in _contextDb.UserRoles select UserRoles).Where(r => r.UserId.Equals(_response.User.Id)).ToList();
            //_response.role = (from Roles in _contextDb.Roles select Roles).Where(x => x.Id.Equals(role.First().RoleId)).ToList().First().Name;
            //generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), _response.User, (from Roles in _contextDb.Roles select Roles).Where(x => x.Id.Equals(role.First().RoleId)).ToList().First());
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
