using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
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
				Email = email ,
				Id = user.Id,
				Roles = await _userManager.GetRolesAsync(user)

			};

			return View(model);
		}
	}
}
