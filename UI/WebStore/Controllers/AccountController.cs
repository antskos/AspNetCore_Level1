using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        #region Register new user
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _logger.LogInformation("Начало процесса регистрации нового пользователя {0}", model.UserName);

            var user = new User
            {
                UserName = model.UserName
            };

            var reg_result = await _userManager.CreateAsync(user, model.Password);
            if (reg_result.Succeeded) 
            {
                _logger.LogInformation("Пользователя {0} успешно зарегистрирован", user.UserName);

                await _userManager.AddToRoleAsync(user, Role.User);

                _logger.LogInformation("Пользователя {0} наделён правами роли {1}", user.UserName, Role.User);

                await _signInManager.SignInAsync(user, false);

                _logger.LogInformation("Пользователя {0} автоматически вошёл в систему после регистрации", user.UserName, Role.User);

                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning( "Ошибка при регистрации нового пользователя {0}\r\n{1}", model.UserName,
                                string.Join(Environment.NewLine, reg_result.Errors.Select(e => e.Description)));

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

            _logger.LogInformation("Попытка входа пользователя {0} в систему", model.UserName);

            if (login_result.Succeeded) 
            {
                _logger.LogInformation("Пользователь {0} вошёл в систему", model.UserName);

                if (Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else 
            {
                _logger.LogWarning("Ошибка в имени пользователя или пароле при попытке входа {0}", model.UserName);

                ModelState.AddModelError(string.Empty, "Неверное имя пользователя, или пароль!");
                return View(model);
            }
        }
        #endregion

        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity.Name;
            
            await _signInManager.SignOutAsync();

            _logger.LogInformation("Пользоваmель {0} вышел из системы", userName);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
