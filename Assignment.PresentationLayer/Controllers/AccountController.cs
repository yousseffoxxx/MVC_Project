global using Microsoft.AspNetCore.Identity;
using Assignment.PresentationLayer.Controllers;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
		public AccountController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
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
    }
}