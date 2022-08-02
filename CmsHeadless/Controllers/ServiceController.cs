using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsHeadless.Models;
using CmsHeadless.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using CmsHeadless.Pages.Category;
using Microsoft.AspNetCore.Identity;
using CmsHeadless.AuthenticationJWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;

namespace CmsHeadless.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly CmsHeadlessDbContext _contextDb;
        public List<Region> RegionAvailable;
        public List<Province> ProvinceAvailable;
        private readonly SignInManager<CmsUser> _signInManager;
        private readonly ResponseApi _response;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private string generatedToken = null;

        public ServiceController(ILogger<ContentController> logger, CmsHeadlessDbContext contextDb, SignInManager<CmsUser> signInManager,
            ResponseApi response, IConfiguration config, ITokenService tokenService, IUserRepository userRepository)
        {
            _logger = logger;
            _contextDb = contextDb;
            _signInManager = signInManager;
            _response = response;
            _config = config;
            _tokenService = tokenService;
            _userRepository = userRepository;

            IQueryable<Region> selectRegionQuery = from Region in _contextDb.Region select Region;
            RegionAvailable = selectRegionQuery.ToList<Region>();
            IQueryable<Models.Province> selectProvinceQuery = from Province in _contextDb.Province select Province;
            ProvinceAvailable = selectProvinceQuery.ToList<Models.Province>();
        }

        [HttpGet]
        public async Task<JsonResult> GetUserAsync(string? mail, string? password, HttpContext httpContext)
        {
            if (mail == null )
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

            var tempUsername = _contextDb.CmsUser.Where(c => c.Email == mail).Select(c => c.UserName).ToList();
            string username = "";

            if (tempUsername.Count > 0)
            {
                username = tempUsername.First();
            }

            var login = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);

            if (login.Succeeded)
            {
                _response.result = true;
                _response.details = "Login effettuato correttamente";
                _response.User = (from User in _contextDb.CmsUser select User).Where(c => c.Email == mail).ToList().First();
                var role = (from UserRoles in _contextDb.UserRoles select UserRoles).Where(r => r.UserId.Equals(_response.User.Id)).ToList();
                _response.role = (from Roles in _contextDb.Roles select Roles).Where(x => x.Id.Equals(role.First().RoleId)).ToList().First().Name;
                generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), _response.User, (from Roles in _contextDb.Roles select Roles).Where(x => x.Id.Equals(role.First().RoleId)).ToList().First());
                if (generatedToken != null)
                {
                    _response.token = generatedToken;
                    AuthTokens token = new AuthTokens();
                    token.UserId = _response.User.Id;
                    token.Token = _response.token;
                    token.CreatedDate = DateTime.Now;
                    var oldToken = (from AuthTokens in _contextDb.AuthTokens select AuthTokens).Where(i => i.UserId.Equals(_response.User.Id)).ToList();
                    if (oldToken.Count!=0)
                    {
                        AuthTokens old = oldToken.First();
                        old.Token = token.Token;
                        
                    }
                    else
                    {
                        _contextDb.AuthTokens.Add(token);
                    }
                    await _contextDb.SaveChangesAsync();
                }
                else
                {
                    _response.result = false;
                    _response.details = "Token generation error occured...";
                }
            }
            else
            {
                _response.result = false;
                _response.details = "Email or password wrong";
            }

            return Json(_response);
        }

        public bool tokenValidation(string mail, string token)
        {
            Debug.Assert(mail != null && token != null);
            _response.User = (from User in _contextDb.CmsUser select User).Where(c => c.Email == mail).ToList().First();
            var tok = ((from AuthTokens in _contextDb.AuthTokens select AuthTokens).Where(i => i.UserId == _response.User.Id && i.CreatedDate.CompareTo(DateTime.Now.AddDays(-5))>=0)).ToList();
            if(tok.Count>0 && tok.First().Token.Equals(token))
            {
                return true;
            }
            return false;
        }
    }
}
