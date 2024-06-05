using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.GrpcServices;

public class GameInterfaceJiliGrpcService(
	ILogger<GameInterfaceJiliGrpcService> logger,
	IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
	ICommandService<PlayerCreateCommand> playerCreateService,
	IGetService<FundInQuery, FundInResult> fundInService,
	IGetService<FundStatusQuery, bool> fundStatusService,
	IGetService<FundOutQuery, FundOutResult> fundOutService,
	IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
	IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
	ICommandService<PlayerLogoutCommand> playerLogoutService,
	IGetService<GameRecordDetailQuery, string> gameDetailService,
	IGetService<PlayerOnlineQuery, bool> playerOnlineService) : GameInterfaceJili.GameInterfaceJiliBase
{
	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName
		});
	}

	public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
	{
		await playerCreateService.ExecuteAsync(
			command: new PlayerCreateCommand(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				PlayerCode: request.PlayerCode),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply()
		{
			State = true
		};
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var result = await fundInService.GetAsync(
			query: new FundInQuery(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				PlayerCode: request.PlayerCode,
				TransactionId: request.TransactionId,
				Amount: request.Amount),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply()
		{
			TransactionId = result.TransactionId,
			CoinBefore = result.CoinBefore,
			CoinAfter = result.CoinAfter,
			CurrencyBefore = result.CurrencyBefore,
			CurrencyAfter = result.CurrencyAfter,
			Resp = result.Resp
		};
	}

	public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
	{
		var result = await fundStatusService.GetAsync(
			query: new FundStatusQuery(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				TransactionId: request.TransactionId),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply()
		{
			State = result
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var result = await fundOutService.GetAsync(
			query: new FundOutQuery(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				PlayerCode: request.PlayerCode,
				TransactionId: request.TransactionId,
				Amount: request.Amount),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply()
		{
			TransactionId = result.TransactionId,
			CoinBefore = result.CoinBefore,
			CoinAfter = result.CoinAfter,
			CurrencyBefore = result.CurrencyBefore,
			CurrencyAfter = result.CurrencyAfter,
			Resp = result.Resp
		};
	}

	public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
	{
		var result = await playerBalanceService.GetAsync(
			query: new PlayerBalanceQuery(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				PlayerCode: request.PlayerCode),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceReply()
		{
			IsPlayerExist = result.IsPlayerExist,
			Balance = result.Balance,
			Online = result.Online
		};
	}

	public override async Task<GameRecordDetailReply> GameRecordDetail(GameRecordDetailRequest request, ServerCallContext context)

	{
		var result = await gameDetailService.GetAsync(
			query: new GameRecordDetailQuery(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				WagersId: request.WagersId,
				Language: request.Language),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameRecordDetailReply()
		{
			Url = result
		};
	}

	public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
	{
		var gameRecords = await gameRecordService.QueryAsync(
			query: new GameRecordQuery(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				StartTime: request.StartTime.ToDateTimeOffset(),
				EndTime: request.EndTime.ToDateTimeOffset(),
				Page: request.Page,
				PageLimit: request.PageLimit),
			cancellationToken: context.CancellationToken)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameRecordResult>();

		var reply = new GameRecordReply();

		if (gameRecords.Any())
		{
			reply.Items.AddRange(gameRecords.Select(transaction => new GameRecordInfo()
			{
				Account = transaction.Account,
				WagersId = transaction.WagersId,
				GameId = transaction.GameId,
				WagersTime = transaction.WagersTime.ToTimestamp(),
				BetAmount = transaction.BetAmount,
				PayoffTime = transaction.PayoffTime.ToTimestamp(),
				PayoffAmount = transaction.PayoffAmount,
				Status = (GameRecordStatus)transaction.Status,
				SettlementTime = transaction.SettlementTime.ToTimestamp(),
				GameCategoryId = transaction.GameCategoryId,
				Type = transaction.Type,
				AgentId = transaction.AgentId,
				Turnover = transaction.Turnover,
				AdditionalData = transaction.AdditionalData
			}));
		}

		return reply;
	}

	public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var result = await gameUrlService.GetAsync(
			query: new GameUrlQuery(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				PlayerCode: request.PlayerCode,
				ExternalGameId: request.ExternalGameId,
				Language: request.Language),
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReply()
		{
			IsPlayerExist = result.IsPlayerExist,
			GameUrl = result.GameUrl
		};
	}

	public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
	{
		await playerLogoutService.ExecuteAsync(
			command: new PlayerLogoutCommand(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				PlayerCode: request.PlayerCode),
			 cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply()
		{
			State = true
		};
	}

	public override async Task<PlayerOnlineReply> PlayerOnline(PlayerOnlineRequest request, ServerCallContext context)
	{
		var online = await playerOnlineService.GetAsync(
			query: new PlayerOnlineQuery(
				ApiUrl: new(request.ApiUrl),
				AgentId: request.AgentId,
				AgentKey: request.AgentKey,
				PlayerCode: request.PlayerCode),
			 cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new PlayerOnlineReply()
		{
			Online = online
		};
	}
}
