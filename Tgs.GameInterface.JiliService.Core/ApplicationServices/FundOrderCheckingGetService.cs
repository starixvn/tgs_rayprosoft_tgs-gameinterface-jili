using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

internal class FundOrderCheckingGetService : IGetService<FundOrderCheckingQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundOrderCheckingGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<bool> GetAsync(FundOrderCheckingQuery command, CancellationToken cancellationToken = default)
        => await _client.CheckTransferByTransactionIdAsync(
            apiUrl: command.ApiUrl,
            agentId: command.AgentId,
            agentKey: command.AgentKey,
            transactionId: command.TransactionId,
            cancellationToken: cancellationToken).ConfigureAwait(false);
}
