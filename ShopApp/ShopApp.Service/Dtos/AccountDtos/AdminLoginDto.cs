using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service.Dtos.AccountDtos
{
    public class AdminLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AdminLoginDtoValidator : AbstractValidator<AdminLoginDto>
    {
        public AdminLoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(25);
        }
    }
}
