using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace WebStore.ViewModels
{
    public class EmployeeViewModel //: IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно быть длинной от 2 до 100 символов")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения")]
        [MinLength(2, ErrorMessage = "Фамилия должна быть длинной не менее 2 символов")]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Required]
        [Range(18,150,ErrorMessage = "Возраст должен быть в пределах от 18 до 150 лет"])
        public int Age { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
            
        //}
    }
}
