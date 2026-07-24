using FluentValidation;
using MedgrupoContacts.Application.DTOs;
using MedgrupoContacts.Application.Interfaces;
using MedgrupoContacts.Domain.Entities;
using MedgrupoContacts.Domain.Exceptions;
using MedgrupoContacts.Domain.Interfaces;

namespace MedgrupoContacts.Application.Services;

public class ContatoService : IContatoService
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IValidator<CreateContatoDto> _createValidator;
    private readonly IValidator<UpdateContatoDto> _updateValidator;

    public ContatoService(
        IContatoRepository contatoRepository,
        IValidator<CreateContatoDto> createValidator,
        IValidator<UpdateContatoDto> updateValidator)
    {
        _contatoRepository = contatoRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<ContatoResponseDto>> ObterTodosAtivosAsync(CancellationToken cancellationToken = default)
    {
        var contatos = await _contatoRepository.ObterTodosAtivosAsync(cancellationToken);
        return contatos.Select(ContatoResponseDto.FromEntity);
    }

    public async Task<ContatoResponseDto?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contato = await _contatoRepository.ObterAtivoPorIdAsync(id, cancellationToken);
        return contato == null ? null : ContatoResponseDto.FromEntity(contato);
    }

    public async Task<ContatoResponseDto> CriarAsync(CreateContatoDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new DomainException(erros);
        }

        var contato = new Contato(dto.Nome, dto.DataNascimento, dto.Sexo);

        await _contatoRepository.AdicionarAsync(contato, cancellationToken);
        await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);

        return ContatoResponseDto.FromEntity(contato);
    }

    public async Task<ContatoResponseDto?> AtualizarAsync(Guid id, UpdateContatoDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var erros = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new DomainException(erros);
        }

        // A listagem, edição e visualização dos contatos deverá considerar apenas contatos ativos
        var contato = await _contatoRepository.ObterAtivoPorIdAsync(id, cancellationToken);
        if (contato == null)
        {
            return null;
        }

        contato.Atualizar(dto.Nome, dto.DataNascimento, dto.Sexo);

        await _contatoRepository.AtualizarAsync(contato, cancellationToken);
        await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);

        return ContatoResponseDto.FromEntity(contato);
    }

    public async Task<bool> AtivarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contato = await _contatoRepository.ObterPorIdAsync(id, cancellationToken);
        if (contato == null) return false;

        contato.Ativar();
        await _contatoRepository.AtualizarAsync(contato, cancellationToken);
        return await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);
    }

    public async Task<bool> DesativarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contato = await _contatoRepository.ObterPorIdAsync(id, cancellationToken);
        if (contato == null) return false;

        contato.Desativar();
        await _contatoRepository.AtualizarAsync(contato, cancellationToken);
        return await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);
    }

    public async Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contato = await _contatoRepository.ObterPorIdAsync(id, cancellationToken);
        if (contato == null) return false;

        await _contatoRepository.RemoverAsync(contato, cancellationToken);
        return await _contatoRepository.SalvarAlteracoesAsync(cancellationToken);
    }
}
