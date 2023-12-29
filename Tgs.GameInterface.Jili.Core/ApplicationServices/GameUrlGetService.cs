using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class GameUrlGetService(
    IGameApiClient client) : IGetService<GameUrlQuery, GameUrlResult>
{
    public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client.LoginWithoutRedirectAsync(
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
