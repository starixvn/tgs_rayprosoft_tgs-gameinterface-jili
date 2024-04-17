using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class BalanceGetService(IGameApiClient client) : IGetService<BalanceQuery, BalanceResult>
{
	public async ValueTask<BalanceResult> GetAsync(BalanceQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.GetMemberInfoAsync(
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
