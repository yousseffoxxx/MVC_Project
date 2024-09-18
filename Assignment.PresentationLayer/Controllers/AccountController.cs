global using Microsoft.AspNetCore.Identity;
using Assignment.PresentationLayer.Controllers;
using PresentationLayer.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
            _signInManager = signInManager;
		}

		public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Create Application User Object
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FristName = model.FristName,
                LastName = model.LastName,
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
                return RedirectToAction(nameof(Login));

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // server side validation
            if (!ModelState.IsValid) return View(model);

            // check if user exists
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            
            if (user is not null)
            {       
                // check Password
                if(_userManager.CheckPasswordAsync(user , model.Password).Result)
                {
                    // login
                    var result = _signInManager.
                        PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;
                    if (result.Succeeded) return RedirectToAction(nameof(HomeController.Index),
                        nameof(HomeController).Replace("Controller", string.Empty));
                }
            
            }
            ModelState.AddModelError(string.Empty, "Incorrect Email Or Password");
            return View(model);
        }

        public new IActionResult SignOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordViewModel model)
        {
            if(!ModelState.IsValid) return View(model);
            // Check if user Exists
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user is not null)
            {
                // Create Reset Password token
                var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                // Create URl to Reset Password
                var url = Url.Action(nameof(ResetPassword), nameof(AccountController).Replace("Controller", string.Empty),
                    new { email = model.Email, Token = token }, Request.Scheme);
                // Creare Emsil Object
                var email = new Email
                {
                    Subject = "Reset Pssword",
                    Body = url!,
                    Recipient = model.Email
                };
                // Send Email
                MailSettings.SendEmail(email);
                // Redirect to Check Your Inbox
                return RedirectToAction(nameof(CheckYourInbox));
            }
            ModelState.AddModelError(string.Empty, "User Not found");
            return View(model);
        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        public IActionResult ResetPassword(string email , string token) 
        {
            if (email is not null || token is null) return BadRequest();
            TempData["Email"] = email;
            TempData["Token"] = token;
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model) 
        {
            model.Token = TempData["Token"]?.ToString()?? string.Empty;
            model.Token = TempData["Email"]?.ToString()?? string.Empty;

			if (!ModelState.IsValid) return View(model);

            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user != null)
            {
                var result = _userManager.ResetPasswordAsync(user , model.Token, model.Password).Result;
                if(!result.Succeeded) return RedirectToAction(nameof(Login));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
			ModelState.AddModelError(string.Empty, "User Not Found");
			return View();
        }
    }
}