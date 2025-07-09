namespace NestedComments.Api.Extensions;

public static class SignalRExtensions
{
    public static IServiceCollection AddAppSignalR(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}