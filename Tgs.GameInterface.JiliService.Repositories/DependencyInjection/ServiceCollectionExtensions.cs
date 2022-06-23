using Tgs.GameInterface.JiliService.Core;
using Tgs.GameInterface.JiliService.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJiLiServiceRepositories(this IServiceCollection services)
            => services
                .AddHttpClient<IGameApiClient, GameApiClient>()
                .Services;
    }
}
