using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.JiliService.Protos;

namespace Tgs.GameInterface.Jili.Client;

internal class GameInterfaceJiliClient : IGameInterfaceJiliClient
{
    private readonly GameInterfaceJiliService.GameInterfaceJiliServiceClient _client;

    public GameInterfaceJiliClient(GameInterfaceJiliService.GameInterfaceJiliServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask CreatePlayerAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
        => await _client.CreatePlayerAsync(new CreatePlayerRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
        => await _client.FundInAsync(new FundRequest()
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
        var reply = await _client.FundOrderCheckingAsync(new FundOrderCheckingRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            TransactionId = transactionId
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        return reply.Exist;
    }

    public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
        => await _client.FundOutAsync(new FundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
            TransactionId = transactionId,
            Amount = amount
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<BalanceReply> GetBalanceAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
        => await _client.GetBalanceAsync(new BalanceRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<string> GetGameDetailUrlAsync(Uri apiUrl, string agentId, string agentKey, string wagersId, string language, CancellationToken cancellationToken = default)
    {
        var reply = await _client
            .GetGameDetailUrlAsync(
                request: new GameDetailRequest()
                {
                    ApiUrl = apiUrl.ToString(),
                    AgentId = agentId,
                    AgentKey = agentKey,
                    WagersId = wagersId,
                    Language = language
                },
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.Url.ToString();
    }

    public async ValueTask<GamePullRecordReply> GetGamePullRecordAsync(Uri apiUrl, string agentId, string agentKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageLimit = 10000, CancellationToken cancellationToken = default)
        => await _client.GetGamePullRecordAsync(new GamePullRecordRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp(),
            Page = page,
            PageLimit = pageLimit
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameUrlReply> GetGameUrlAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string externalGameId, string language, CancellationToken cancellationToken = default)
        => await _client.GetGameUrlAsync(new GameUrlRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
            ExternalGameId = externalGameId,
            Language = language
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask KickPlayerAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
        => await _client.KickPlayerAsync(new KickPlayerRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentId = agentId,
            AgentKey = agentKey,
            PlayerCode = playerCode,
        }, cancellationToken: cancellationToken).ConfigureAwait(false);
}
