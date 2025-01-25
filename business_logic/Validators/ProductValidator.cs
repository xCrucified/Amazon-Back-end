using business_logic.DTOs;
using business_logic.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Validators
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

            RuleFor(x => x.Discount)
                .LessThan(100)
                .GreaterThan(1)
                .NotEmpty();

            RuleFor(x => x.Description).
                MaximumLength(1200).NotEmpty();

            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1).NotEmpty();

            RuleFor(x => x.CategoryId).NotEmpty().GreaterThanOrEqualTo(1);
            
        }
    }
}
