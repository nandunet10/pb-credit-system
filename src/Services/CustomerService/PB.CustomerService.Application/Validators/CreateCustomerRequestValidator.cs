using FluentValidation;
using PB.CustomerService.Application.DTOs;
using PB.CustomerService.Application.Helpers;

namespace PB.CustomerService.Application.Validators
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");

            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage("CPF é obrigatório")
                .Must(CustomerServiceHelper.BeValidCPF).WithMessage("CPF inválido");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Data de nascimento é obrigatória")
                .Must(CustomerServiceHelper.BeAtLeast18YearsOld).WithMessage("Cliente deve ter no mínimo 18 anos");

            RuleFor(x => x.Income)
                .GreaterThanOrEqualTo(0).WithMessage("Renda não pode ser negativa");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefone é obrigatório");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Endereço é obrigatório")
                .SetValidator(new AddressDtoValidator());
        }
    }
}
