using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        #region Register new user
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                UserName = model.UserName
            };

            var reg_Result = await _userManager.CreateAsync(user);
            if (reg_Result.Succeeded) 
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in reg_Result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }
        #endregion

        #region Login user

        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel{ReturnUrl = ReturnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if (!ModelState.IsValid) return View(model);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        public IActionResult Logout() => View();

        public IActionResult AccessDenied() => View();
    }
}
