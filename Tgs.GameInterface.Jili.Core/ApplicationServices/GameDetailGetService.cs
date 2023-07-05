using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class GameDetailGetService : IGetService<GameDetailQuery, string>
{
    private readonly IGameApiClient _client;

    public GameDetailGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string> GetAsync(GameDetailQuery query, CancellationToken cancellationToken = default)
        => _client
        .GetGameDetailUrlAsync(
            apiUrl: query.ApiUrl,
            agentId: query.AgentId,
            agentKey: query.AgentKey,
            wagersId: query.WagersId,
            lang: query.Language,
            cancellationToken: cancellationToken);
}
