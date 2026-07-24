using MedgrupoContacts.Domain.Entities;
using MedgrupoContacts.Domain.Enums;

namespace MedgrupoContacts.Application.DTOs;

public class ContatoResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public SexoEnum Sexo { get; set; }
    public string SexoDescricao => Sexo.ToString();
    public int Idade { get; set; }
    public bool Ativo { get; set; }

    public static ContatoResponseDto FromEntity(Contato contato)
    {
        return new ContatoResponseDto
        {
            Id = contato.Id,
            Nome = contato.Nome,
            DataNascimento = contato.DataNascimento,
            Sexo = contato.Sexo,
            Idade = contato.Idade,
            Ativo = contato.Ativo
        };
    }
}
