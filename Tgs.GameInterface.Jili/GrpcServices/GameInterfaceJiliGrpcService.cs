using System.Transactions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.JiliService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.GrpcServices;

public class GameInterfaceJiliGrpcService(
	ILogger<GameInterfaceJiliGrpcService> logger,
	 IGetService<BalanceQuery, BalanceResult> balanceService,
	 ICommandService<CreatePlayerCommand> createPlayerService,
	 IGetService<FundInQuery, FundInResult> fundInService,
	 IGetService<FundOrderCheckingQuery, bool> fundOrderCheckingService,
	 IGetService<FundOutQuery, FundOutResult> fundOutService,
	 IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
	 IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
	 ICommandService<KickMemberCommand> kickMemberService,
	 IGetService<GameDetailQuery, string> gameDetailService) : GameInterfaceJiliService.GameInterfaceJiliServiceBase
{
	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName
		});
	}

	public override async Task<BaseReply> CreatePlayer(CreatePlayerRequest request, ServerCallContext context)
	{
		await createPlayerService.ExecuteAsync(new CreatePlayerCommand(
			ApiUrl: new(request.ApiUrl),
			AgentId: request.AgentId,
			AgentKey: request.AgentKey,
			PlayerCode: request.PlayerCode)).ConfigureAwait(false);

		return new BaseReply()
		{
			Success = true
		};
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var result = await fundInService.GetAsync(new FundInQuery(
			 ApiUrl: new(request.ApiUrl),
			 AgentId: request.AgentId,
			 AgentKey: request.AgentKey,
			 PlayerCode: request.PlayerCode,
			 TransactionId: request.TransactionId,
			 Amount: request.Amount)).ConfigureAwait(false);

		return new FundReply()
		{
			Success = result.Success,
			Message = result.Message ?? String.Empty,
			TransactionId = result.TransactionId,
			CoinBefore = result.CoinBefore,
			CoinAfter = result.CoinAfter,
			CurrencyBefore = result.CurrencyBefore,
			CurrencyAfter = result.CurrencyAfter
		};
	}

	public override async Task<FundOrderCheckingReply> FundOrderChecking(FundOrderCheckingRequest request, ServerCallContext context)
	{
		var result = await fundOrderCheckingService.GetAsync(new FundOrderCheckingQuery(
			 ApiUrl: new(request.ApiUrl),
			 AgentId: request.AgentId,
			 AgentKey: request.AgentKey,
			 TransactionId: request.TransactionId)).ConfigureAwait(false);

		return new FundOrderCheckingReply()
		{
			Exist = result
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var result = await fundOutService.GetAsync(new FundOutQuery(
			 ApiUrl: new(request.ApiUrl),
			 AgentId: request.AgentId,
			 AgentKey: request.AgentKey,
			 PlayerCode: request.PlayerCode,
			 TransactionId: request.TransactionId,
			 Amount: request.Amount)).ConfigureAwait(false);

		return new FundReply()
		{
			Success = result.Success,
			Message = result.Message ?? String.Empty,
			TransactionId = result.TransactionId,
			CoinBefore = result.CoinBefore,
			CoinAfter = result.CoinAfter,
			CurrencyBefore = result.CurrencyBefore,
			CurrencyAfter = result.CurrencyAfter
		};
	}

	public override async Task<BalanceReply> GetBalance(BalanceRequest request, ServerCallContext context)
	{
		var result = await balanceService.GetAsync(new BalanceQuery(
			 ApiUrl: new(request.ApiUrl),
			 AgentId: request.AgentId,
			 AgentKey: request.AgentKey,
			 PlayerCode: request.PlayerCode)).ConfigureAwait(false);

		return new BalanceReply()
		{
			Success = result.Success,
			Message = result.Message ?? String.Empty,
			IsPlayerExist = result.IsPlayerExist,
			Balance = result.Balance
		};
	}

	public override async Task<GameDetailReply> GetGameDetailUrl(GameDetailRequest request, ServerCallContext context)

	{
		var result = await gameDetailService
			.GetAsync(
				query: new GameDetailQuery(
					ApiUrl: new(request.ApiUrl),
					AgentId: request.AgentId,
					AgentKey: request.AgentKey,
					WagersId: request.WagersId,
					Language: request.Language),
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameDetailReply()
		{
			Url = result ?? String.Empty,
		};
	}

	public override async Task<GamePullRecordReply> GetGamePullRecord(GamePullRecordRequest request, ServerCallContext context)
	{
		var gameRecords = await gameRecordService.QueryAsync(new GameRecordQuery(
			ApiUrl: new(request.ApiUrl),
			AgentId: request.AgentId,
			AgentKey: request.AgentKey,
			StartTime: request.StartTime.ToDateTimeOffset(),
			EndTime: request.EndTime.ToDateTimeOffset(),
			Page: request.Page,
			PageLimit: request.PageLimit))
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameRecordResult>();

		var reply = new GamePullRecordReply();

		if (gameRecords.Any())
		{
			reply.Items.AddRange(gameRecords.Select(transaction => new GamePullRecord()
			{
				Account = transaction.Account,
				WagersId = transaction.WagersId,
				GameId = transaction.GameId,
				WagersTime = transaction.WagersTime.ToTimestamp(),
				BetAmount = transaction.BetAmount,
				PayoffTime = transaction.PayoffTime.ToTimestamp(),
				PayoffAmount = transaction.PayoffAmount,
				Status = (GamePullRecordStatus)transaction.Status,
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

	public override async Task<GameUrlReply> GetGameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var result = await gameUrlService.GetAsync(new GameUrlQuery(
			 ApiUrl: new(request.ApiUrl),
			 AgentId: request.AgentId,
			 AgentKey: request.AgentKey,
			 PlayerCode: request.PlayerCode,
			 ExternalGameId: request.ExternalGameId,
			 Language: request.Language)).ConfigureAwait(false);

		return new GameUrlReply()
		{
			Success = result.Success,
			Message = result.Message ?? String.Empty,
			IsPlayerExist = result.IsPlayerExist,
			GameUrl = result.GameUrl
		};
	}

	public override async Task<BaseReply> KickPlayer(KickPlayerRequest request, ServerCallContext context)
	{
		await kickMemberService.ExecuteAsync(new KickMemberCommand(
			   ApiUrl: new(request.ApiUrl),
			   AgentId: request.AgentId,
			   AgentKey: request.AgentKey,
			   PlayerCode: request.PlayerCode)).ConfigureAwait(false);

		return new BaseReply()
		{
			Success = true
		};
	}
}
