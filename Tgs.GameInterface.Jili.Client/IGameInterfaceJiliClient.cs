using Tgs.GameInterface.JiliService.Protos;

namespace Tgs.GameInterface.Jili.Client;

public interface IGameInterfaceJiliClient
{
    ValueTask CreatePlayerAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<bool> FundOrderCheckingAsync(Uri apiUrl, string agentId, string agentKey, string transactionId, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<BalanceReply> GetBalanceAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask<GamePullRecordReply> GetGamePullRecordAsync(Uri apiUrl, string agentId, string agentKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageLimit = 100, CancellationToken cancellationToken = default);

    ValueTask<GameUrlReply> GetGameUrlAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string externalGameId, string language, CancellationToken cancellationToken = default);

    ValueTask KickPlayerAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);
}
