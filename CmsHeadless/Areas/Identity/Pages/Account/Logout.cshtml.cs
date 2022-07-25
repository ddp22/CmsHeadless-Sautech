// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CmsHeadless.Controllers;
using CmsHeadless.Models;
using CmsHeadless.Data;
using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly LogListController _logController;
        private readonly CmsHeadlessDbContext _contextDb;
        private readonly User _user;
        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, LogListController logController, CmsHeadlessDbContext contextDb)
        {
            _signInManager = signInManager;
            _logger = logger;
            _logController = logController;
            _contextDb = contextDb;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            //var temp = _contextDb.User.Where(c => c.Email == User.Identity.Name).Select(c => c.Id).ToList();

            //string userId;

            //if (temp.Count > 0)
            //{
            //    userId = temp[temp.Count - 1];
            //}
            //else
            //{
            //    userId = "Not defined";
            //}

            _logController.SaveLog(User.Identity.Name, LogListController.LogoutSuccessfulCode, "L'utente " + User.Identity.Name + " ha eseguito il logout.", "Logout successful.", HttpContext);
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
