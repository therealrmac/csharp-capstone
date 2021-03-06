﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ChatItUp.Models;
using ChatItUp.Models.ManageViewModels;
using ChatItUp.Services;
using ChatItUp.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ChatItUp.Models.ViewModels;

namespace ChatItUp.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory,
          ApplicationDbContext context,
          IHostingEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _context = context;
            _environment = environment;
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            //LINQ FOR GETTING A LIST OF FRIENDS WHERE THE USER IS EQUAL TO THE CURRENT USER
            var completeFriendList = await _context.Relation.Include("Friend").Where(x => x.User == user && x.Connected == true).ToListAsync();
            //END

            //LINQ FOR GETTING A LIST OF FRIENDS WHERE THE USER IS EQUAL TO ID THAT WAS PASSED IN
            var FriendList = await _context.Relation.Include("User").Where(x => x.Friend == user && x.Connected == true).ToListAsync();
            //END

            //LINQ FOR GETTING A COUNT ALL OF THE CURRENT USERS THREADPOSTS
            var posts = _context.ThreadPost.Where(x => x.user == user);
            //END

            //LINQ FOR GETTING A LIST OF THREADS CRAETED BY THE CURRENT USER
            var threadsCretaed = await _context.Thread.Where(x => x.User == user).ToListAsync();
            //END

            //BINDING THESE VARIABLES TO THE VIEW MODEL
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                ApplicationUser = user,
                friendList = completeFriendList,
                totalPosts = posts,
                friendList2= FriendList,
                totalThreads= threadsCretaed
            };
            return View(model);
        }

        //GET LIST OF FRIEND REQUESTS 
        public async Task<IActionResult> Requests()
        {
            RequestViewModel request = new RequestViewModel();
            var user = await GetCurrentUserAsync();
            if(user == null)
            {
                return NotFound();
            }

            //LINQ FOR GETTING A ROW IN RELATION TABLE WHERE THE FRIENDID IS EQUAL TO THE CURRENT USER
            var incomingRequests =  _context.Relation.Include("User").Where(x => x.Friend == user && x.Connected == null);
            //END

            return View(incomingRequests);

        }



        //ACCEPT FRIEND REQUEST POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ConfirmRequest(string user)
        {
            var currentUser = await GetCurrentUserAsync();


            ApplicationUser userFriend = await _context.ApplicationUser.Where(u => u.Id == user).SingleOrDefaultAsync();

            //LINQ FOR FINDING ANY RELATION THAT MATCHES WHERE THE FRIEND COLUMN ON RELATION TABLE EQUALS THE CURRENT USER AND IS NOT CONNECTED
            var connectedRelation = _context.Relation.Include("User").Single(x => x.Friend == currentUser && x.Connected == null);
            //END

            //SETTING THE CONNECTED VALUE TO TRUE TO NOW SAY THAT THESE TWO USERS ARE FRIENDS
            connectedRelation.Connected = true;

            //UPDATE THESE CHANGES AND SAVE THEM
            _context.Update(connectedRelation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Requests", "Manage");

        }

        //Decline FRIEND REQUEST POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeclineRequest(string user)
        {
            var currentUser = await GetCurrentUserAsync();

            ApplicationUser userFriend = await _context.ApplicationUser.Where(u => u.Id == user).SingleOrDefaultAsync();

            //LINQ FOR FINDING ANY RELATION THAT MATCHES WHERE THE FRIEND COLUMN ON RELATION TABLE EQUALS THE CURRENT USER AND IS NOT CONNECTED
            var connectedRelation = _context.Relation.Include("User").Single(x => x.Friend == currentUser && x.Connected == null);
            //END
           
            //TO "DECLINE" THE REQUEST, THE ENTIRE ROW WILL BE DELETED FROM THE DATABASE  AND SAVED
            _context.Remove(connectedRelation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Requests", "Manage");

        }

        //GET User Post Feed
        public async Task<IActionResult> Feed()
        {
            FriendFeedViewModel ffVM = new FriendFeedViewModel();
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }

            //LINQ FOR GETTING A LIST OF POSTS MADE BY A FRIEND FROM THE FRIEND COLUMN IN REALATION TABLE WHERE THE USER COLUMN IN RELATION TABLE IS THE CURRENT USER
            var yourFriends = await _context.Relation
                .Include(x => x.Friend)
                 .ThenInclude(y => y.ThreadPosts)
                .Where(x => x.User == user && x.Connected == true).ToListAsync();

            foreach(var item in yourFriends)
            {
               ffVM.friendPost1.AddRange(item.Friend.ThreadPosts);
            }
            //END

            //LINQ FOR GETTING A LIST OF POSTS MADE BY FRIENDS FROM THE USER COLUMN IN REALTION WHERE FRIEND COLUMN IS THE CURRENT USER
            var yourFriends2= await _context.Relation
                .Include(x => x.User)
                 .ThenInclude(y => y.ThreadPosts)
                .Where(x => x.Friend == user && x.Connected == true).ToListAsync();
            //END
            foreach (var item in yourFriends2)
            {
                ffVM.friendPost1.AddRange(item.User.ThreadPosts);
            }

            ffVM.friendPost1 = ffVM.friendPost1.OrderByDescending(x => x.dateCreatd).ToList();
            //BIND THESE NEW VARIABLES TO THE VIEW MODEL


            return View(ffVM);
           
        }



        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //GET: /Manage/ManageLogins
        [HttpGet]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var otherLogins = _signInManager.GetExternalAuthenticationSchemes().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback), "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        //
        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = ManageMessageId.Error;
            if (result.Succeeded)
            {
                message = ManageMessageId.AddLoginSuccess;
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        //BY: RYAN MCCARTY
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            //GET THE CURRENT USER
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound("Error");
            }

            IndexViewModel User = new IndexViewModel()
            {
                ApplicationUser = user
            };


            if (User.ApplicationUser == null)
            {
                return NotFound();
            }
            return View();
        }


        //BY: RYAN MCCARTY
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile( IndexViewModel IndVM)
        {
            if (IndVM == null)
            {
                throw new ArgumentNullException(nameof(IndVM));
            }
            //PASS IN ALL THE VALUES FROM THE VIEW MODEL THAT WILL BE UPDATED
            var user = await GetCurrentUserAsync();
            user.Firstname = IndVM.ApplicationUser.Firstname;
            user.Lastname = IndVM.ApplicationUser.Lastname;


            if (ModelState.IsValid)
            {
                try
                {
                   
                        var filename = ContentDispositionHeaderValue
                                        .Parse(IndVM.profileImg.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        filename = _environment.WebRootPath + $@"\Profile\{IndVM.profileImg.FileName.Split('\\').Last()}";

                        using (var fileStream = new FileStream(filename, FileMode.Create))
                        {
                            await IndVM.profileImg.CopyToAsync(fileStream);
                            user.ProfileImage = $@"\Profile\{IndVM.profileImg.FileName.Split('\\').Last()}";
                        }
                    

                        var filename2 = ContentDispositionHeaderValue
                                        .Parse(IndVM.bannerImg.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        filename2 = _environment.WebRootPath + $@"\Profile\{
                            IndVM.bannerImg.FileName.Split('\\').Last()}";

                        using (var fileStream = new FileStream(filename2, FileMode.Create))
                        {
                            await IndVM.bannerImg.CopyToAsync(fileStream);
                            user.BannerImage = $@"\Profile\{
                                IndVM.bannerImg.FileName.Split('\\').Last()}";
                        }
                    

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(IndVM.ApplicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        
        public async Task<IActionResult> Search(string searchFor, string searchText)
        {
            UserListViewModel viewModel = new UserListViewModel();

            if (!String.IsNullOrEmpty(searchText) && searchFor.Equals("People"))
            {
                viewModel.user = await _context.ApplicationUser.Where(s => s.Firstname.ToLower().Contains(searchText.ToLower()) || s.Lastname.ToLower().Contains(searchText.ToLower())).ToListAsync();
            }
            else if (!String.IsNullOrEmpty(searchText) && searchFor.Equals("Forum"))
            {
                viewModel.forum = await _context.Forum.Where(l => l.ThreadTitles.ToLower().Contains(searchText.ToLower())).ToListAsync();
            }

            return View(viewModel);
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion


        //CHECKS TO SEE IF THE USERID EXISTS 
        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
