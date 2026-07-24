using FluentAssertions;
using MedgrupoContacts.Application.DTOs;
using MedgrupoContacts.Application.Validators;
using MedgrupoContacts.Domain.Enums;
using Xunit;

namespace MedgrupoContacts.Tests.Validators;

public class CreateContatoDtoValidatorTests
{
    private readonly CreateContatoDtoValidator _validator;

    public CreateContatoDtoValidatorTests()
    {
        _validator = new CreateContatoDtoValidator();
    }

    [Fact]
    public void Validar_DtoValido_DevePassarSemErros()
    {
        // Arrange
        var dto = new CreateContatoDto
        {
            Nome = "Juliana Costa",
            DataNascimento = DateTime.Today.AddYears(-30),
            Sexo = SexoEnum.Feminino
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validar_DataFutura_DeveRetornarErroDeValidacao()
    {
        // Arrange
        var dto = new CreateContatoDto
        {
            Nome = "Juliana Costa",
            DataNascimento = DateTime.Today.AddDays(5),
            Sexo = SexoEnum.Feminino
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DataNascimento");
    }
}
