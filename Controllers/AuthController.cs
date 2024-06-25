//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Razor.TagHelpers;
//using WebUI.DTOs;
//using WebUI.Models;

//namespace WebUI.Controllers
//{
//	public class AuthController : Controller
//	{
//		private readonly UserManager<AppUser> _userManager;
//		private readonly SignInManager<AppUser> _signInManager;

//		public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
//		{
//			_userManager = userManager;
//			_signInManager = signInManager;
//		}
//		[HttpGet]
//		public IActionResult Login()
//		{
//			if(User.Identity.IsAuthenticated)
//			{
//				return RedirectToAction("Index", "Home");
//			}
//			return View();
//		}
//		[HttpPost]
//		public async Task<IActionResult> Login(LoginDTO loginDTO)
//		{
//			var checkEmail = await _userManager.FindByEmailAsync(loginDTO.Email);
//			if(checkEmail != null)
//			{
//				ModelState.AddModelError("Error", "Email or Password is incorrect!");
//				return View();
//			}
//			Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(checkEmail.UserName, loginDTO.Password, loginDTO.RememberMe, true);
//			if (result.Succeeded)
//			{
//				return RedirectToAction("Index", "Home");
//			}
//			else
//			{
//				ModelState.AddModelError("Error", "Email or Password is incorrect!");
//				return View();
//			}
//		}
//		[HttpGet]
//		public async Task<IActionResult> Register (RegisterDTO registerDTO)
//		{
//			var checkEmail = await _userManager.FindByEmailAsync (registerDTO.Email);
//			if(checkEmail is not null)
//			{
//				ModelState.AddModelError("Error", "Email or Password is incorrect!");
//				return View();
//			}
//			AppUser newUser = new()
//			{
//				Email = registerDTO.Email,
//				FirstName = registerDTO.FirstName,
//				LastName = registerDTO.LastName,
//				UserName = registerDTO.Email,
//			};
//			var result = await _userManager.CreateAsync(newUser, registerDTO.Password);	
//			if (result.Succeeded)
//			{
//				return RedirectToAction(nameof(Login));
//			}
//			else
//			{
//				foreach (var error in result.Errors)
//				{
//					ModelState.AddModelError("Error" , error.Description);	
//				}
//				return View();
//			}
//		}
//	}
//}





using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.DTOs;
using WebUI.Models;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
	public class AuthController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO loginDTO)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = await _userManager.FindByEmailAsync(loginDTO.Email);
			if (user == null)
			{
				ModelState.AddModelError("Error", "Email or Password is incorrect!");
				return View();
			}

			var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDTO.Password, loginDTO.RememberMe, true);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("Error", "Email or Password is incorrect!");
				return View();
			}
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = await _userManager.FindByEmailAsync(registerDTO.Email);
			if (user != null)
			{
				ModelState.AddModelError("Error", "Email is already in use.");
				return View();
			}

			var newUser = new AppUser
			{
				Email = registerDTO.Email,
				UserName = registerDTO.Email,
				FirstName = registerDTO.FirstName,
				LastName = registerDTO.LastName,
			};

			var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
			if (result.Succeeded)
			{
				return RedirectToAction(nameof(Login));
			}
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("Error", error.Description);
				}
				return View();
			}
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction(nameof(Login));
		}
	}
}





