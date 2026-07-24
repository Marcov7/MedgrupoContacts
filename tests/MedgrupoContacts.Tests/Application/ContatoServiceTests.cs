using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MedgrupoContacts.Application.DTOs;
using MedgrupoContacts.Application.Services;
using MedgrupoContacts.Domain.Entities;
using MedgrupoContacts.Domain.Enums;
using MedgrupoContacts.Domain.Interfaces;
using Moq;
using Xunit;

namespace MedgrupoContacts.Tests.Application;

public class ContatoServiceTests
{
    private readonly Mock<IContatoRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateContatoDto>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateContatoDto>> _updateValidatorMock;
    private readonly ContatoService _service;

    public ContatoServiceTests()
    {
        _repositoryMock = new Mock<IContatoRepository>();
        _createValidatorMock = new Mock<IValidator<CreateContatoDto>>();
        _updateValidatorMock = new Mock<IValidator<UpdateContatoDto>>();

        _createValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateContatoDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _updateValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateContatoDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _service = new ContatoService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    [Fact]
    public async Task ObterTodosAtivosAsync_DeveRetornarApenasContatosAtivos()
    {
        // Arrange
        var contatos = new List<Contato>
        {
            new Contato("Contato 1", DateTime.Today.AddYears(-25), SexoEnum.Masculino),
            new Contato("Contato 2", DateTime.Today.AddYears(-30), SexoEnum.Feminino)
        };

        _repositoryMock.Setup(r => r.ObterTodosAtivosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(contatos);

        // Act
        var resultado = await _service.ObterTodosAtivosAsync();

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Select(c => c.Nome).Should().Contain(new[] { "Contato 1", "Contato 2" });
        _repositoryMock.Verify(r => r.ObterTodosAtivosAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_ComDadosValidos_DeveSalvarERetornarDto()
    {
        // Arrange
        var dto = new CreateContatoDto
        {
            Nome = "Lucas Mendes",
            DataNascimento = DateTime.Today.AddYears(-22),
            Sexo = SexoEnum.Masculino
        };

        _repositoryMock.Setup(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var resultado = await _service.CriarAsync(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be("Lucas Mendes");
        resultado.Idade.Should().Be(22);
        resultado.Ativo.Should().BeTrue();

        _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Contato>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_ContatoNaoEncontradoOuInativo_DeveRetornarNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateContatoDto
        {
            Nome = "Novo Nome",
            DataNascimento = DateTime.Today.AddYears(-25),
            Sexo = SexoEnum.Outro
        };

        _repositoryMock.Setup(r => r.ObterAtivoPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contato?)null);

        // Act
        var resultado = await _service.AtualizarAsync(id, dto);

        // Assert
        resultado.Should().BeNull();
        _repositoryMock.Verify(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DesativarAsync_ContatoExistente_DeveRetornarTrue()
    {
        // Arrange
        var contato = new Contato("Fernanda", DateTime.Today.AddYears(-28), SexoEnum.Feminino);
        _repositoryMock.Setup(r => r.ObterPorIdAsync(contato.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contato);
        _repositoryMock.Setup(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var resultado = await _service.DesativarAsync(contato.Id);

        // Assert
        resultado.Should().BeTrue();
        contato.Ativo.Should().BeFalse();
        _repositoryMock.Verify(r => r.AtualizarAsync(contato, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExcluirAsync_ContatoExistente_DeveRemoverERetornarTrue()
    {
        // Arrange
        var contato = new Contato("Roberto", DateTime.Today.AddYears(-40), SexoEnum.Masculino);
        _repositoryMock.Setup(r => r.ObterPorIdAsync(contato.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contato);
        _repositoryMock.Setup(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var resultado = await _service.ExcluirAsync(contato.Id);

        // Assert
        resultado.Should().BeTrue();
        _repositoryMock.Verify(r => r.RemoverAsync(contato, It.IsAny<CancellationToken>()), Times.Once);
    }
}
