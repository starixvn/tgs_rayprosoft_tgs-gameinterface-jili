using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, GameUrlResult>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.LoginWithoutRedirectAsync(
            apiUrl: query.ApiUrl,
            agentId: query.AgentId,
            agentKey: query.AgentKey,
            playerCode: query.PlayerCode,
            gameId: query.ExternalGameId,
            lang: query.Language,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new GameUrlResult(
            Success: result.Success,
            Message: result.Message,
            IsPlayerExist: result.IsPlayerExist,
            GameUrl: result.GameUrl);
    }
}
