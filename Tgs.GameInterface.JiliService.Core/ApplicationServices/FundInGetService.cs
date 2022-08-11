using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundInResult>
{
    private readonly IGameApiClient _client;

    public FundInGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundInResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.ExchangeTransferByAgentIdAsync(
            apiUrl: query.ApiUrl,
            agentId: query.AgentId,
            agentKey: query.AgentKey,
            playerCode: query.PlayerCode,
            transactionId: query.TransactionId,
            transferType: TransferType.FundIn,
            amount: query.Amount,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new FundInResult(
            Success: result.Success,
            Message: result.Message,
            TransactionId: result.TransactionId,
            CoinBefore: result.CoinBefore,
            CoinAfter: result.CoinAfter,
            CurrencyBefore: result.CurrencyBefore,
            CurrencyAfter: result.CurrencyAfter);
    }
}
