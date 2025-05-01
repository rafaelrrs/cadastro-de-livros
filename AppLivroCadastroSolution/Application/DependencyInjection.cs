using AppLivroCadastro.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AppLivroCadastro.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<Services.LivroService>();
            services.AddScoped<AutorService>();
            services.AddScoped<AssuntoService>();

            return services;
        }
    }
}
