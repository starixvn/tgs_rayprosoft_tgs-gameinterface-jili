using Tgs.GameInterface.JiliService.Core.Models;

namespace Tgs.GameInterface.JiliService.Core;

public interface IGameApiClient
{
    ValueTask<bool> CheckTransferByTransactionIdAsync(Uri apiUrl, string agentId, string agentKey, string transactionId, CancellationToken cancellationToken = default);

    ValueTask CreateMemberAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask<ExchangeTransferByAgentIdResult> ExchangeTransferByAgentIdAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, TransferType transferType, decimal amount, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GetBetRecordTransaction> GetBetRecordByTimeAsync(Uri apiUrl, string agentId, string agentKey, DateTime startTime, DateTime endTime, int page = 1, int pageLimit = 100, CancellationToken cancellationToken = default);

    ValueTask<GetMemberInfoResult> GetMemberInfoAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask KickMemberAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask<LoginWithoutRedirectResult> LoginWithoutRedirectAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string gameId, string lang, CancellationToken cancellationToken = default);
}
