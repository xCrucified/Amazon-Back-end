using BLL.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validators
{
    public class ReviewValidator : AbstractValidator<ReviewDto>
    {
        public ReviewValidator() 
        {
            RuleFor(x => x.Rate).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5).NotEmpty();
            RuleFor(x => x.PostDate).NotEmpty();
            RuleFor(x => x.ReviewText).MaximumLength(3500).MinimumLength(1).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}
