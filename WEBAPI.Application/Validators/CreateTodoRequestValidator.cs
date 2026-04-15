using FluentValidation;
using WEBAPI.Application.DTOs;

namespace WEBAPI.Application.Validators;

public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Baslik alani zorunludur.")
            .MaximumLength(200).WithMessage("Baslik en fazla 200 karakter olabilir.");
    }
}
