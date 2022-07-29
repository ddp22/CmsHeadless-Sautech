using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CmsHeadless.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CmsHeadless.Controllers;
using Microsoft.AspNetCore.Authorization;
using CmsHeadless.AuthenticationJWT;

namespace CmsHeadlessApi.Controllers
{
    public class UserController : Controller
    {
        private readonly CmsHeadlessDbContext _contextDb;
        private readonly ILogger<ContentController> _logger;
        private readonly SignInManager<CmsUser> _signInManager;
        private readonly ServiceController _serviceController;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        public UserController(CmsHeadlessDbContext contextDb, ILogger<ContentController> logger, ServiceController serviceController, SignInManager<CmsUser> signInManager,
            IConfiguration config, ITokenService tokenService, IUserRepository userRepository)
        {
            _contextDb = contextDb;
            _logger = logger;
            _serviceController = serviceController;
            _signInManager = signInManager;
            _config = config;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<JsonResult> LoginUserAsync(string? mail, string? password)
        {
            return Json(_serviceController.GetUserAsync(mail, password, HttpContext).Result.Value);
        }
    }
}
