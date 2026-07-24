using MedgrupoContacts.Domain.Enums;

namespace MedgrupoContacts.Application.DTOs;

public class CreateContatoDto
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public SexoEnum Sexo { get; set; }
}
