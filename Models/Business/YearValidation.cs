using System;
using System.ComponentModel.DataAnnotations;
using Repository.Models.DatabaseModels;
using Repository.ViewModels;

namespace Repository.Models.Business
{
    public class YearValidation: ValidationAttribute
    {
        private readonly int _year;

        public YearValidation(int year)
        {
            _year = year;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (RegisterViewModel)validationContext.ObjectInstance;

            if (user.Year < _year || user.Year > DateTime.Now.Year)
            {
                return new ValidationResult("Неподходящий возраст");
            }

            return ValidationResult.Success;
        }
    }
}