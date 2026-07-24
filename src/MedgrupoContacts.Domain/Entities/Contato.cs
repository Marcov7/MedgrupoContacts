using MedgrupoContacts.Domain.Enums;
using MedgrupoContacts.Domain.Exceptions;

namespace MedgrupoContacts.Domain.Entities;

public class Contato
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public SexoEnum Sexo { get; private set; }
    public bool Ativo { get; private set; }

    /// <summary>
    /// A idade é processada dinamicamente em tempo de execução com base na Data de Nascimento e na data atual.
    /// </summary>
    public int Idade => CalcularIdade(DataNascimento, DateTime.Today);

    // Construtor protegido exigido pelo EF Core
    protected Contato() { }

    public Contato(string nome, DateTime dataNascimento, SexoEnum sexo)
    {
        Id = Guid.NewGuid();
        Ativo = true;
        SetNome(nome);
        SetDataNascimento(dataNascimento);
        SetSexo(sexo);
    }

    public void Atualizar(string nome, DateTime dataNascimento, SexoEnum sexo)
    {
        if (!Ativo)
            throw new DomainException("Não é possível editar um contato inativo.");

        SetNome(nome);
        SetDataNascimento(dataNascimento);
        SetSexo(sexo);
    }

    public void Ativar()
    {
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    private void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do contato é obrigatório.");

        if (nome.Trim().Length < 3 || nome.Trim().Length > 100)
            throw new DomainException("O nome deve ter entre 3 e 100 caracteres.");

        Nome = nome.Trim();
    }

    private void SetDataNascimento(DateTime dataNascimento)
    {
        var dataApenas = dataNascimento.Date;
        var hoje = DateTime.Today;

        if (dataApenas > hoje)
            throw new DomainException("A data de nascimento não poderá ser maior que a data de hoje.");

        int idadeCalculada = CalcularIdade(dataApenas, hoje);

        if (idadeCalculada == 0)
            throw new DomainException("A idade não poderá ser igual a 0.");

        if (idadeCalculada < 18)
            throw new DomainException("O contato deverá ser maior de idade (mínimo 18 anos).");

        DataNascimento = dataApenas;
    }

    private void SetSexo(SexoEnum sexo)
    {
        if (!Enum.IsDefined(typeof(SexoEnum), sexo))
            throw new DomainException("Sexo informado é inválido.");

        Sexo = sexo;
    }

    public static int CalcularIdade(DateTime dataNascimento, DateTime dataReferencia)
    {
        var hoje = dataReferencia.Date;
        var nascimento = dataNascimento.Date;

        int idade = hoje.Year - nascimento.Year;

        if (nascimento.Date > hoje.AddYears(-idade))
        {
            idade--;
        }

        return idade;
    }
}
