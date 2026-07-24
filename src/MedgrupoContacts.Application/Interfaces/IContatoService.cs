using MedgrupoContacts.Application.DTOs;

namespace MedgrupoContacts.Application.Interfaces;

public interface IContatoService
{
    Task<IEnumerable<ContatoResponseDto>> ObterTodosAtivosAsync(CancellationToken cancellationToken = default);
    Task<ContatoResponseDto?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ContatoResponseDto> CriarAsync(CreateContatoDto dto, CancellationToken cancellationToken = default);
    Task<ContatoResponseDto?> AtualizarAsync(Guid id, UpdateContatoDto dto, CancellationToken cancellationToken = default);
    Task<bool> AtivarAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DesativarAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default);
}
