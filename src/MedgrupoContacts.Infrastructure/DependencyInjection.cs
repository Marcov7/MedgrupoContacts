using MedgrupoContacts.Domain.Interfaces;
using MedgrupoContacts.Infrastructure.Data;
using MedgrupoContacts.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedgrupoContacts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=(localdb)\\mssqllocaldb;Database=MedgrupoContactsDb;Trusted_Connection=True;MultipleActiveResultSets=true;";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IContatoRepository, ContatoRepository>();

        return services;
    }
}
