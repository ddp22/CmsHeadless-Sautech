using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsHeadless.Data;
using CmsHeadless.Models;
using CmsHeadless.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using CmsHeadless.Pages.Category;
using Microsoft.AspNetCore.Identity;

namespace CmsHeadless.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public List<Region> RegionAvailable;
        public List<Province> ProvinceAvailable;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ResponseApi _response;

        public ServiceController(ILogger<ContentController> logger, CmsHeadlessDbContext contextDb, SignInManager<IdentityUser> signInManager, ResponseApi response)
        {
            _logger = logger;
            _contextDb = contextDb;
            _signInManager = signInManager;
            _response = response;

            IQueryable<Region> selectRegionQuery = from Region in _contextDb.Region select Region;
            RegionAvailable = selectRegionQuery.ToList<Region>();
            IQueryable<Models.Province> selectProvinceQuery = from Province in _contextDb.Province select Province;
            ProvinceAvailable = selectProvinceQuery.ToList<Models.Province>();
        }

        public async Task<JsonResult> GetUserAsync(string? username, string? password)
        {
            if (username == null )
            {
                _response.result = false;
                _response.details = "Email field is empty";
                return Json(_response);
            }

            else if (password == null) {
                _response.result = false;
                _response.details = "Password field is empty";
                return Json(_response);
            }

            var login = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);

            if (login.Succeeded)
            {
                _response.result = true;
                _response.details = "Login effettuato correttamente";
                _response.User = (from User in _contextDb.User select User).Where(c => c.Email == username).ToList().First();

            }

            else
            {
                _response.result = false;
                _response.details = "Email or password wrong";
            }

            return Json(_response);
        }
    }
}
