using MedgrupoContacts.Domain.Entities;

namespace MedgrupoContacts.Domain.Interfaces;

public interface IContatoRepository
{
    Task<Contato?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Contato?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Contato>> ObterTodosAtivosAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Contato contato, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Contato contato, CancellationToken cancellationToken = default);
    Task RemoverAsync(Contato contato, CancellationToken cancellationToken = default);
    Task<bool> SalvarAlteracoesAsync(CancellationToken cancellationToken = default);
}
