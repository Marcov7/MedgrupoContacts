using FluentValidation;
using MedgrupoContacts.Application.DTOs;
using MedgrupoContacts.Application.Interfaces;
using MedgrupoContacts.Application.Services;
using MedgrupoContacts.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace MedgrupoContacts.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IContatoService, ContatoService>();
        services.AddScoped<IValidator<CreateContatoDto>, CreateContatoDtoValidator>();
        services.AddScoped<IValidator<UpdateContatoDto>, UpdateContatoDtoValidator>();

        return services;
    }
}
