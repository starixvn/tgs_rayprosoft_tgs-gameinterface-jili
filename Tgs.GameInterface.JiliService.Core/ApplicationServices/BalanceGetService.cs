using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

internal class BalanceGetService : IGetService<BalanceQuery, BalanceResult>
{
    private readonly IGameApiClient _client;

    public BalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<BalanceResult> GetAsync(BalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.GetMemberInfoAsync(
            query.ApiUrl,
            query.AgentId,
            query.AgentKey,
            query.PlayerCode,
            cancellationToken).ConfigureAwait(false);

        return new BalanceResult(
             Success: result.Success,
             Message: result.Message,
             IsPlayerExist: result.IsPlayerExist,
             Balance: result.Balance);
    }
}
