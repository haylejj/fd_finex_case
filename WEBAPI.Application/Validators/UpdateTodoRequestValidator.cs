using FluentValidation;
using WEBAPI.Application.DTOs;

namespace WEBAPI.Application.Validators;

public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id alani 0'dan buyuk olmalidir.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Baslik alani zorunludur.")
            .MaximumLength(200).WithMessage("Baslik en fazla 200 karakter olabilir.");
    }
}
