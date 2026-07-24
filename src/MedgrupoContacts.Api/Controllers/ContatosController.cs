using MedgrupoContacts.Application.DTOs;
using MedgrupoContacts.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedgrupoContacts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContatosController : ControllerBase
{
    private readonly IContatoService _contatoService;

    public ContatosController(IContatoService contatoService)
    {
        _contatoService = contatoService;
    }

    /// <summary>
    /// Listar todos os contatos ativos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContatoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodosAtivos(CancellationToken cancellationToken)
    {
        var contatos = await _contatoService.ObterTodosAtivosAsync(cancellationToken);
        return Ok(contatos);
    }

    /// <summary>
    /// Visualizar detalhes de um contato ativo pelo ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContatoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var contato = await _contatoService.ObterAtivoPorIdAsync(id, cancellationToken);
        if (contato == null)
        {
            return NotFound(new { mensagem = "Contato não encontrado ou inativo." });
        }

        return Ok(contato);
    }

    /// <summary>
    /// Criar novo contato
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContatoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CreateContatoDto dto, CancellationToken cancellationToken)
    {
        var contatoCriado = await _contatoService.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = contatoCriado.Id }, contatoCriado);
    }

    /// <summary>
    /// Editar um contato ativo
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ContatoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] UpdateContatoDto dto, CancellationToken cancellationToken)
    {
        var contatoAtualizado = await _contatoService.AtualizarAsync(id, dto, cancellationToken);
        if (contatoAtualizado == null)
        {
            return NotFound(new { mensagem = "Contato não encontrado ou inativo para edição." });
        }

        return Ok(contatoAtualizado);
    }

    /// <summary>
    /// Ativar um contato
    /// </summary>
    [HttpPatch("{id:guid}/ativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(Guid id, CancellationToken cancellationToken)
    {
        var sucesso = await _contatoService.AtivarAsync(id, cancellationToken);
        if (!sucesso)
        {
            return NotFound(new { mensagem = "Contato não encontrado." });
        }

        return NoContent();
    }

    /// <summary>
    /// Desativar um contato
    /// </summary>
    [HttpPatch("{id:guid}/desativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken cancellationToken)
    {
        var sucesso = await _contatoService.DesativarAsync(id, cancellationToken);
        if (!sucesso)
        {
            return NotFound(new { mensagem = "Contato não encontrado." });
        }

        return NoContent();
    }

    /// <summary>
    /// Excluir contato permanentemente
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid id, CancellationToken cancellationToken)
    {
        var sucesso = await _contatoService.ExcluirAsync(id, cancellationToken);
        if (!sucesso)
        {
            return NotFound(new { mensagem = "Contato não encontrado." });
        }

        return NoContent();
    }
}
