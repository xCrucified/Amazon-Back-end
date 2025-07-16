using BLL.DTOs;
using BLL.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator() 
        {
            RuleFor(x => x.Price)
                .GreaterThan(1)
                .NotEmpty();

            RuleFor(x => x.Name)
                .MinimumLength(3)
                .NotEmpty();


            RuleFor(x => x.Description).
                MaximumLength(1200).NotEmpty();


            RuleFor(x => x.SubcategoryId).NotEmpty().GreaterThanOrEqualTo(1);
            
        }
    }
}
