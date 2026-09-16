using FluentValidation; using CRN.ProductApi.Application.DTOs;
namespace CRN.ProductApi.Application.Validators;
public class CreateProductValidator : AbstractValidator<CreateProductDto>{public CreateProductValidator(){RuleFor(x=>x.ProductName).NotEmpty().WithMessage("Product name is required").MaximumLength(255);RuleFor(x=>x.Quantities).NotNull();RuleForEach(x=>x.Quantities).GreaterThanOrEqualTo(0);}}
public class UpdateProductValidator : AbstractValidator<UpdateProductDto>{public UpdateProductValidator(){RuleFor(x=>x.ProductName).NotEmpty().WithMessage("Product name is required").MaximumLength(255);RuleFor(x=>x.Quantities).NotNull();RuleForEach(x=>x.Quantities).GreaterThanOrEqualTo(0);}}
