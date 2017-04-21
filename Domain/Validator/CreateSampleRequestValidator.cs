using Domain.Requests;
using FluentValidation;

namespace Domain.Validator
{
    public class CreateSampleRequestValidator : AbstractValidator<CreateSampleRequest>
    {
        public CreateSampleRequestValidator()
        {
            RuleFor(r => r)
                .NotNull().WithMessage("Request empty");

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Name should not be empty");

            RuleFor(r => r.Size)
                .NotEmpty().WithMessage("Size should not be empty");
        }
    }
}