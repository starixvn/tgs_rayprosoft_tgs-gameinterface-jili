using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Core.ApplicationServices
{
    internal class FundOutGetService : IGetService<FundOutQuery, FundOutResult>
    {
        private readonly IGameApiClient _client;

        public FundOutGetService(IGameApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async ValueTask<FundOutResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
        {
            var result = await _client.ExchangeTransferByAgentIdAsync(
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
}
