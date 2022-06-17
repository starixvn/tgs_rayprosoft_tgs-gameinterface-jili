using Microsoft.Extensions.Options;
using Tgs.GameInterface.JiliService.Client;
using Tgs.GameInterface.JiliService.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceJiliServiceClient(
        this IServiceCollection services,
        Action<GameInterfaceJiliServiceClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfaceJiliServiceClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceJiliService.GameInterfaceJiliServiceClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceJiliServiceClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceJiliServiceGrpc, GameInterfaceJiliServiceGrpc>();
}
