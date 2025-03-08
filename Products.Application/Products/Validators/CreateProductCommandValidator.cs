using FluentValidation;
using Products.Application.Products.Commands;

namespace Products.Application.Products.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters")
            .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Product name can only contain letters and numbers");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero")
            .ScalePrecision(2, 18).WithMessage("Price must have up to 2 decimal places");

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");
    }
}