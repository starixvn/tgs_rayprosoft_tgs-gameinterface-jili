using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.JiliService.Protos;

namespace Tgs.GameInterface.JiliService.Client;

internal class GameInterfaceJiliServiceGrpc : IGameInterfaceJiliServiceGrpc
{
    private readonly GameInterfaceJiliService.GameInterfaceJiliServiceClient _client;

    public GameInterfaceJiliServiceGrpc(GameInterfaceJiliService.GameInterfaceJiliServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask CreatePlayerAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
        => await _client.CreatePlayerAsync(new GameInterfaceJiliServiceCreatePlayerRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameInterfaceJiliServiceFundReply> FundInAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
        => await _client.FundInAsync(new GameInterfaceJiliServiceFundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
            TransactionId = transactionId,
            Amount = amount
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<bool> FundOrderCheckingAsync(Uri apiUrl, string agentId, string agentKey, string transactionId, CancellationToken cancellationToken = default)
    {
        var reply = await _client.FundOrderCheckingAsync(new GameInterfaceJiliServiceFundOrderCheckingRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            TransactionId = transactionId
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        return reply.Exist;
    }

    public async ValueTask<GameInterfaceJiliServiceFundReply> FundOutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
        => await _client.FundOutAsync(new GameInterfaceJiliServiceFundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
            TransactionId = transactionId,
            Amount = amount
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameInterfaceJiliServiceBalanceReply> GetBalanceAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
        => await _client.GetBalanceAsync(new GameInterfaceJiliServiceBalanceRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameInterfaceJiliServiceGamePullRecordReply> GetGamePullRecordAsync(Uri apiUrl, string agentId, string agentKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageLimit = 100, CancellationToken cancellationToken = default)
        => await _client.GetGamePullRecordAsync(new GameInterfaceJiliServiceGamePullRecordRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp(),
            Page = page,
            PageLimit = pageLimit
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameInterfaceJiliServiceGameUrlReply> GetGameUrlAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string externalGameId, string language, CancellationToken cancellationToken = default)
        => await _client.GetGameUrlAsync(new GameInterfaceJiliServiceGameUrlRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
            ExternalGameId = externalGameId,
            Language = language
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask KickPlayerAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
        => await _client.KickPlayerAsync(new GameInterfaceJiliServiceKickPlayerRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
        }, cancellationToken: cancellationToken).ConfigureAwait(false);
}
