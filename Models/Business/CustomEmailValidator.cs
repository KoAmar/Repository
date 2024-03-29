﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Repository.Models.DatabaseModels;

namespace Repository.Models.Business
{
    public class CustomEmailValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            var errors = new List<IdentityError>();

            var email = user.Email.ToLower();
            if (email.EndsWith("@spam.com"))
                errors.Add(new IdentityError
                {
                    Description = "Данный домен находится в спам-базе. Выберите другой почтовый сервис"
                });
            if (user.UserName.Contains("admin"))
                errors.Add(new IdentityError
                {
                    Description = "Ник пользователя не должен содержать слово 'admin'"
                });

            try
            {
                var m = new MailAddress(email);
            }
            catch (FormatException)
            {
                errors.Add(new IdentityError
                {
                    Description = "Некорректный формат email"
                });
            }

            return Task.FromResult(errors.Count == 0
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray()));
        }
    }
}