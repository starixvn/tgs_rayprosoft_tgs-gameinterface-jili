using Tgs.GameInterface.Jili.Protos;

namespace Tgs.GameInterface.Jili.Client;

public interface IGameInterfaceJiliClient
{
	ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

	ValueTask PlayerCreateAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

	ValueTask PlayerLogoutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(Uri apiUrl, string agentId, string agentKey, string transactionId, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string externalGameId, string language, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string agentId, string agentKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageLimit = 10000, CancellationToken cancellationToken = default);

	ValueTask<GameRecordDetailReply> GameRecordDetailAsync(Uri apiUrl, string agentId, string agentKey, string wagersId, string language, CancellationToken cancellationToken = default);

	ValueTask<bool> PlayerOnlineAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);
}
