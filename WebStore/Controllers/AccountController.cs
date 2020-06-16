using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Register(RegisterUserViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }
        #endregion

        public IActionResult Login() => View();

        public IActionResult Logout() => View();

        public IActionResult AccessDenied() => View();
    }
}
