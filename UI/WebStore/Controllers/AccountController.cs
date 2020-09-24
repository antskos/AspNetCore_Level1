using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

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

            var reg_result = await _userManager.CreateAsync(user, model.Password);
            if (reg_result.Succeeded) 
            {
                await _userManager.AddToRoleAsync(user, Role.User);

                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in reg_result.Errors)
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

            var login_result = await _signInManager.PasswordSignInAsync(model.UserName,
                                                                        model.Password,
                                                                        model.RememberMe,
                                                                        true);
            if (login_result.Succeeded) 
            {
                if (Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя, или пароль!");
                return View(model);
            }
        }
        #endregion

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
