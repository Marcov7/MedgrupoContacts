using FluentValidation;
using MedgrupoContacts.Application.DTOs;

namespace MedgrupoContacts.Application.Validators;

public class CreateContatoDtoValidator : AbstractValidator<CreateContatoDto>
{
    public CreateContatoDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.")
            .NotEqual("Maria Fora").WithMessage("O nome 'Maria Fora' não é permitido.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("A data de nascimento não poderá ser maior que a data de hoje.");

        RuleFor(x => x.Sexo)
            .IsInEnum().WithMessage("Sexo informado é inválido.");

    }
}
