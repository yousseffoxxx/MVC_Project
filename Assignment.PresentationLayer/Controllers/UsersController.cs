using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UsersController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				var users = await _userManager.Users.Select(u => new UserViewModel
				{
					FirstName = u.FristName,
					LastName = u.LastName,
					UserName = u.UserName,
					Email = u.Email,
					Id = u.Id,
					Roles = _userManager.GetRolesAsync(u).GetAwaiter().GetResult()
				}).ToListAsync();
				return View(users);
			}
			var user = await _userManager.FindByEmailAsync(email);

			if (user is null) return View(Enumerable.Empty<UserViewModel>());

			var model = new UserViewModel 
			{
				FirstName = user.FristName,
				LastName = user.LastName,
				UserName = user.UserName,
				Email = user.Email ,
				Id = user.Id,
				Roles = await _userManager.GetRolesAsync(user)
			};

			return View(model);
		}

        public async Task<IActionResult> Details(string id, string viewName = nameof(Details))
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            var userModel = new UserViewModel
            {
                FirstName = user.FristName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return View(viewName,userModel);
        }

		public async Task<IActionResult> Edit(string id) => await Details(id , nameof(Edit));

		[HttpPost]
        public async Task<IActionResult> Edit(string id , UserViewModel model)
		{
			if(id != model.Id) return BadRequest();
			if (!ModelState.IsValid) return View(model);

			try
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is null) return NotFound();
				user.FristName = model.FirstName;
				user.LastName = model.LastName;
				await _userManager.UpdateAsync(user);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(model);
		}

        public async Task<IActionResult> Delete(string id) => await Details(id , nameof(Delete));

		[ActionName("Delete")]
		[HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
		{
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return NotFound();
                await _userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();

        }
    }
}