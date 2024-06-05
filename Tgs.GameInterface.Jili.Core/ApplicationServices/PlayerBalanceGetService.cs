using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.GetMemberInfoAsync(
			query.ApiUrl,
			query.AgentId,
			query.AgentKey,
			query.PlayerCode,
			cancellationToken).ConfigureAwait(false);

		return new PlayerBalanceResult(
			 IsPlayerExist: result.IsPlayerExist,
			 Balance: result.Balance,
			 Online: result.Online);
	}
}
