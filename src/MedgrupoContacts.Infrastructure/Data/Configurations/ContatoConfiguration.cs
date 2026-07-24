using MedgrupoContacts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedgrupoContacts.Infrastructure.Data.Configurations;

public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("Contatos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.DataNascimento)
            .IsRequired();

        builder.Property(c => c.Sexo)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.Ativo)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Ignore(c => c.Idade);
    }
}
