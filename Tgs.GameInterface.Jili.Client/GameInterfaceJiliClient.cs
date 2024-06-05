using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.Jili.Protos;

namespace Tgs.GameInterface.Jili.Client;

internal class GameInterfaceJiliClient(
	GameInterfaceJili.GameInterfaceJiliClient client) : IGameInterfaceJiliClient
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client.CheckingServiceAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask PlayerCreateAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
		=> await client.PlayerCreateAsync(
			request: new PlayerCreateRequest()
			{
				ApiUrl = apiUrl.ToString(),
				AgentId = agentId,
				AgentKey = agentKey,
				PlayerCode = playerCode
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
		=> await client.PlayerBalanceAsync(
			request: new PlayerBalanceRequest()
			{
				ApiUrl = apiUrl.ToString(),
				AgentId = agentId,
				AgentKey = agentKey,
				PlayerCode = playerCode,
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask PlayerLogoutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
		=> await client.PlayerLogoutAsync(
			request: new PlayerLogoutRequest()
			{
				ApiUrl = apiUrl.ToString(),
				AgentId = agentId,
				AgentKey = agentKey,
				PlayerCode = playerCode,
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
		=> await client.FundInAsync(
			 request: new FundRequest()
			 {
				 ApiUrl = apiUrl.ToString(),
				 AgentId = agentId,
				 AgentKey = agentKey,
				 PlayerCode = playerCode,
				 TransactionId = transactionId,
				 Amount = amount
			 },
			 cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string agentId, string agentKey, string transactionId, CancellationToken cancellationToken = default)
	{
		var reply = await client.FundStatusAsync(
			 request: new FundStatusRequest()
			 {
				 ApiUrl = apiUrl.ToString(),
				 AgentId = agentId,
				 AgentKey = agentKey,
				 TransactionId = transactionId
			 },
			 cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.State;
	}

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
		=> await client.FundOutAsync(
			  request: new FundRequest()
			  {
				  ApiUrl = apiUrl.ToString(),
				  AgentId = agentId,
				  AgentKey = agentKey,
				  PlayerCode = playerCode,
				  TransactionId = transactionId,
				  Amount = amount
			  },
			  cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string externalGameId, string language, CancellationToken cancellationToken = default)
		=> await client.GameUrlAsync(
			request: new GameUrlRequest()
			{
				ApiUrl = apiUrl.ToString(),
				AgentId = agentId,
				AgentKey = agentKey,
				PlayerCode = playerCode,
				ExternalGameId = externalGameId,
				Language = language
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async ValueTask<GameRecordDetailReply> GameRecordDetailAsync(Uri apiUrl, string agentId, string agentKey, string wagersId, string language, CancellationToken cancellationToken = default)
		=> await client.GameRecordDetailAsync(
			request: new GameRecordDetailRequest()
			{
				ApiUrl = apiUrl.ToString(),
				AgentId = agentId,
				AgentKey = agentKey,
				WagersId = wagersId,
				Language = language
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);

	public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string agentId, string agentKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageLimit = 10000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var reply = await client.GameRecordAsync(
			request: new GameRecordRequest()
			{
				ApiUrl = apiUrl.ToString(),
				AgentId = agentId,
				AgentKey = agentKey,
				StartTime = startTime.ToTimestamp(),
				EndTime = endTime.ToTimestamp(),
				Page = page,
				PageLimit = pageLimit
			},
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		foreach (var info in reply.Items)
		{
			yield return info;
		}

		yield break;
	}

	public async ValueTask<bool> PlayerOnlineAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
		=> (await client.PlayerOnlineAsync(
			request: new PlayerOnlineRequest()
			{
				ApiUrl = apiUrl.ToString(),
				AgentId = agentId,
				AgentKey = agentKey,
				PlayerCode = playerCode,
			},
			cancellationToken: cancellationToken)
		.ConfigureAwait(false)).Online;
}
