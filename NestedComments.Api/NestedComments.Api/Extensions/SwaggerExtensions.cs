using Microsoft.OpenApi.Models;

namespace NestedComments.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NestedComments API", Version = "v1" });
            });
        }

        return services;
    }
}
