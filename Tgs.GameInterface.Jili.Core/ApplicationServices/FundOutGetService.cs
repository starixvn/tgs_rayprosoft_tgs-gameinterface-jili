using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class FundOutGetService(
	IGameApiClient client) : IGetService<FundOutQuery, FundOutResult>
{
	public async ValueTask<FundOutResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.ExchangeTransferByAgentIdAsync(
			apiUrl: query.ApiUrl,
			agentId: query.AgentId,
			agentKey: query.AgentKey,
			playerCode: query.PlayerCode,
			transactionId: query.TransactionId,
			transferType: TransferType.FundOut,
			amount: query.Amount,
			cancellationToken: cancellationToken).ConfigureAwait(false);

		return new FundOutResult(
			Success: result.Success,
			Message: result.Message,
			TransactionId: result.TransactionId,
			CoinBefore: result.CoinBefore,
			CoinAfter: result.CoinAfter,
			CurrencyBefore: result.CurrencyBefore,
			CurrencyAfter: result.CurrencyAfter);
	}
}
