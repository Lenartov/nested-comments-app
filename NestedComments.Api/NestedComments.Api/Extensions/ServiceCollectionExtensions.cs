using Microsoft.EntityFrameworkCore;
using NestedComments.Api.Data;
using NestedComments.Api.Services;
using NestedComments.Api.Services.Interfaces;
using NestedComments.Api.Settings;

namespace NestedComments.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

        services.Configure<CaptchaSettings>(config.GetSection("CaptchaSettings"));

        services.AddMemoryCache();

        services.AddScoped<ICaptchaService, CaptchaService>();
        services.AddScoped<ICommentSanitizer, CommentSanitizer>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ICommentService, CachedCommentService>();

        services.AddSingleton<ICommentCacheManager, CommentCacheManager>();
        services.AddSingleton<ICommentQueueService, CommentQueueService>();

        services.AddHostedService<QueuedCommentProcessor>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        return services;
    }
}
