using Microsoft.AspNetCore.Mvc;
using CmsHeadless.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CmsHeadless.Controllers;

namespace CmsHeadlessApi.Controllers
{
    public class UserController : Controller
    {
        private readonly CmsHeadlessDbContext _contextDb;
        private readonly ILogger<ContentController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ServiceController _serviceController;
        public UserController(CmsHeadlessDbContext contextDb, ILogger<ContentController> logger, ServiceController serviceController, SignInManager<IdentityUser> signInManager)
        {
            _contextDb = contextDb;
            _logger = logger;
            _serviceController = serviceController;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<JsonResult> LoginUserAsync(string? mail, string? password)
        {
           return Json(_serviceController.GetUserAsync(mail, password).Result.Value);
        }
    }
}
