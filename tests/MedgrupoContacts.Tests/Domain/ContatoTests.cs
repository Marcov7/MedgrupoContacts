using FluentAssertions;
using MedgrupoContacts.Domain.Entities;
using MedgrupoContacts.Domain.Enums;
using MedgrupoContacts.Domain.Exceptions;
using Xunit;

namespace MedgrupoContacts.Tests.Domain;

public class ContatoTests
{
    [Fact]
    public void CriarContato_ComDadosValidos_DeveInstanciarComSucesso()
    {
        // Arrange
        var dataNascimento = DateTime.Today.AddYears(-20);

        // Act
        var contato = new Contato("João Silva", dataNascimento, SexoEnum.Masculino);

        // Assert
        contato.Should().NotBeNull();
        contato.Nome.Should().Be("João Silva");
        contato.DataNascimento.Should().Be(dataNascimento.Date);
        contato.Sexo.Should().Be(SexoEnum.Masculino);
        contato.Ativo.Should().BeTrue();
        contato.Idade.Should().Be(20);
    }

    [Fact]
    public void CriarContato_MenorDeIdade_DeveLancarDomainException()
    {
        // Arrange (17 anos)
        var dataNascimento = DateTime.Today.AddYears(-17);

        // Act
        Action act = () => new Contato("Maria Santos", dataNascimento, SexoEnum.Feminino);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*maior de idade*");
    }

    [Fact]
    public void CriarContato_DataNascimentoFutura_DeveLancarDomainException()
    {
        // Arrange
        var dataFutura = DateTime.Today.AddDays(1);

        // Act
        Action act = () => new Contato("Carlos Oliveira", dataFutura, SexoEnum.Masculino);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*não poderá ser maior que a data de hoje*");
    }

    [Fact]
    public void CriarContato_NomeInvalido_DeveLancarDomainException()
    {
        // Act
        Action actVazio = () => new Contato("", DateTime.Today.AddYears(-25), SexoEnum.Outro);
        Action actCurto = () => new Contato("Ab", DateTime.Today.AddYears(-25), SexoEnum.Outro);

        // Assert
        actVazio.Should().Throw<DomainException>();
        actCurto.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("2000-05-15", "2020-05-15", 20)] // Dia exato do aniversário
    [InlineData("2000-05-15", "2020-05-14", 19)] // Véspera do aniversário
    [InlineData("2000-05-15", "2020-05-16", 20)] // Dia seguinte ao aniversário
    [InlineData("2004-02-29", "2024-02-29", 20)] // Ano Bissexto
    public void CalcularIdade_DeveRetornarIdadeExata(string nascimentoStr, string referenciaStr, int idadeEsperada)
    {
        // Arrange
        var nascimento = DateTime.Parse(nascimentoStr);
        var referencia = DateTime.Parse(referenciaStr);

        // Act
        int idade = Contato.CalcularIdade(nascimento, referencia);

        // Assert
        idade.Should().Be(idadeEsperada);
    }

    [Fact]
    public void DesativarEAtivar_DeveAlterarStatusCorretamente()
    {
        // Arrange
        var contato = new Contato("Ana Paula", DateTime.Today.AddYears(-30), SexoEnum.Feminino);

        // Act & Assert
        contato.Ativo.Should().BeTrue();

        contato.Desativar();
        contato.Ativo.Should().BeFalse();

        contato.Ativar();
        contato.Ativo.Should().BeTrue();
    }

    [Fact]
    public void Atualizar_ContatoInativo_DeveLancarDomainException()
    {
        // Arrange
        var contato = new Contato("Pedro Souza", DateTime.Today.AddYears(-25), SexoEnum.Masculino);
        contato.Desativar();

        // Act
        Action act = () => contato.Atualizar("Pedro Souza Novo", DateTime.Today.AddYears(-26), SexoEnum.Masculino);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*inativo*");
    }
}
