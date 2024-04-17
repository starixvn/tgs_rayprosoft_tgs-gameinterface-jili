using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class FundOrderCheckingGetService(
	IGameApiClient client) : IGetService<FundOrderCheckingQuery, bool>
{
	public async ValueTask<bool> GetAsync(FundOrderCheckingQuery command, CancellationToken cancellationToken = default)
		=> await client.CheckTransferByTransactionIdAsync(
			apiUrl: command.ApiUrl,
			agentId: command.AgentId,
			agentKey: command.AgentKey,
			transactionId: command.TransactionId,
			cancellationToken: cancellationToken).ConfigureAwait(false);
}
