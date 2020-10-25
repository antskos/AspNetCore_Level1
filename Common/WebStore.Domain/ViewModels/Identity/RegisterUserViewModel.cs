using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels.Identity
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(256, ErrorMessage = "Длина строки должна быть меньше 256 символов")]
        [Display(Name = "Имя пользователя")]
        [Remote("IsNameFree","Account")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare(nameof(Password), ErrorMessage = "Пароли должны совпадать!")]
        public string ConfirmPassword { get; set; }
    }
}
