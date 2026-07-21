using FluentValidation;
using ProductManagement.Application.DTOs.Product;

namespace ProductManagement.Application.Validators.Product;

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(255)
            .WithMessage("Product name cannot exceed 255 characters.")
            .MinimumLength(3)
            .WithMessage("Product name must be at least 3 characters long.");
    }
}