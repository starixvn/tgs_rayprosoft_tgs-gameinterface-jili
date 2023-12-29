using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class GameDetailGetService(
    IGameApiClient client) : IGetService<GameDetailQuery, string>
{
    public ValueTask<string> GetAsync(GameDetailQuery query, CancellationToken cancellationToken = default)
        => client
        .GetGameDetailUrlAsync(
            apiUrl: query.ApiUrl,
            agentId: query.AgentId,
            agentKey: query.AgentKey,
            wagersId: query.WagersId,
            lang: query.Language,
            cancellationToken: cancellationToken);
}
