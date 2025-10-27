using FluentValidation;
using PB.CustomerService.Application.DTOs;

namespace PB.CustomerService.Application.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Street).NotEmpty().WithMessage("Rua é obrigatória");
            RuleFor(x => x.Number).NotEmpty().WithMessage("Número é obrigatório");
            RuleFor(x => x.Neighborhood).NotEmpty().WithMessage("Bairro é obrigatório");
            RuleFor(x => x.City).NotEmpty().WithMessage("Cidade é obrigatória");
            RuleFor(x => x.State).NotEmpty().WithMessage("Estado é obrigatório");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("CEP é obrigatório");
        }
    }
}
