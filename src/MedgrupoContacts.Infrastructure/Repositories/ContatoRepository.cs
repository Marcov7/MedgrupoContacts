using MedgrupoContacts.Domain.Entities;
using MedgrupoContacts.Domain.Interfaces;
using MedgrupoContacts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedgrupoContacts.Infrastructure.Repositories;

public class ContatoRepository : IContatoRepository
{
    private readonly ApplicationDbContext _context;

    public ContatoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Contato?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Contatos.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Contato?> ObterAtivoPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Contatos.FirstOrDefaultAsync(c => c.Id == id && c.Ativo, cancellationToken);
    }

    public async Task<IEnumerable<Contato>> ObterTodosAtivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Contatos.Where(c => c.Ativo).ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Contato contato, CancellationToken cancellationToken = default)
    {
        await _context.Contatos.AddAsync(contato, cancellationToken);
    }

    public Task AtualizarAsync(Contato contato, CancellationToken cancellationToken = default)
    {
        _context.Contatos.Update(contato);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Contato contato, CancellationToken cancellationToken = default)
    {
        _context.Contatos.Remove(contato);
        return Task.CompletedTask;
    }

    public async Task<bool> SalvarAlteracoesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
