using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.JiliService.Core.ApplicationServices;
using Tgs.GameInterface.JiliService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.GrpcServices;

public class GameInterfaceJiliGrpcService : GameInterfaceJiliService.GameInterfaceJiliServiceBase
{
    private readonly IGetService<BalanceQuery, BalanceResult> _balanceService;
    private readonly ICommandService<CreatePlayerCommand> _createPlayerService;
    private readonly IGetService<FundInQuery, FundInResult> _fundInService;
    private readonly IGetService<FundOrderCheckingQuery, bool> _fundOrderCheckingService;
    private readonly IGetService<FundOutQuery, FundOutResult> _fundOutService;
    private readonly IQueryService<GameRecordQuery, GameRecordResult> _gameRecordService;
    private readonly IGetService<GameUrlQuery, GameUrlResult> _gameUrlService;
    private readonly ICommandService<KickMemberCommand> _kickMemberService;

    public GameInterfaceJiliGrpcService(
         IGetService<BalanceQuery, BalanceResult> balanceService,
         ICommandService<CreatePlayerCommand> createPlayerService,
         IGetService<FundInQuery, FundInResult> fundInService,
         IGetService<FundOrderCheckingQuery, bool> fundOrderCheckingService,
         IGetService<FundOutQuery, FundOutResult> fundOutService,
         IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
         IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
         ICommandService<KickMemberCommand> kickMemberService
         )
    {
        _balanceService = balanceService ?? throw new ArgumentNullException(nameof(balanceService));
        _createPlayerService = createPlayerService ?? throw new ArgumentNullException(nameof(createPlayerService));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOrderCheckingService = fundOrderCheckingService ?? throw new ArgumentNullException(nameof(fundOrderCheckingService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _gameRecordService = gameRecordService ?? throw new ArgumentNullException(nameof(gameRecordService));
        _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
        _kickMemberService = kickMemberService ?? throw new ArgumentNullException(nameof(kickMemberService));
    }

    public override async Task<GameInterfaceJiliServiceReply> CreatePlayer(GameInterfaceJiliServiceCreatePlayerRequest request, ServerCallContext context)
    {
        await _createPlayerService.ExecuteAsync(new CreatePlayerCommand(
            ApiUrl: new(request.ApiUrl),
            AgentId: request.AgentId,
            AgentKey: request.AgentKey,
            PlayerCode: request.PlayerCode)).ConfigureAwait(false);

        return new GameInterfaceJiliServiceReply()
        {
            Success = true
        };
    }

    public override async Task<GameInterfaceJiliServiceFundReply> FundIn(GameInterfaceJiliServiceFundRequest request, ServerCallContext context)
    {
        var result = await _fundInService.GetAsync(new FundInQuery(
             ApiUrl: new(request.ApiUrl),
             AgentId: request.AgentId,
             AgentKey: request.AgentKey,
             PlayerCode: request.PlayerCode,
             TransactionId: request.TransactionId,
             Amount: request.Amount)).ConfigureAwait(false);

        return new GameInterfaceJiliServiceFundReply()
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

    public override async Task<GameInterfaceJiliServiceFundOrderCheckingReply> FundOrderChecking(GameInterfaceJiliServiceFundOrderCheckingRequest request, ServerCallContext context)
    {
        var result = await _fundOrderCheckingService.GetAsync(new FundOrderCheckingQuery(
             ApiUrl: new(request.ApiUrl),
             AgentId: request.AgentId,
             AgentKey: request.AgentKey,
             TransactionId: request.TransactionId)).ConfigureAwait(false);

        return new GameInterfaceJiliServiceFundOrderCheckingReply()
        {
            Exist = result
        };
    }

    public override async Task<GameInterfaceJiliServiceFundReply> FundOut(GameInterfaceJiliServiceFundRequest request, ServerCallContext context)
    {
        var result = await _fundOutService.GetAsync(new FundOutQuery(
             ApiUrl: new(request.ApiUrl),
             AgentId: request.AgentId,
             AgentKey: request.AgentKey,
             PlayerCode: request.PlayerCode,
             TransactionId: request.TransactionId,
             Amount: request.Amount)).ConfigureAwait(false);

        return new GameInterfaceJiliServiceFundReply()
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

    public override async Task<GameInterfaceJiliServiceBalanceReply> GetBalance(GameInterfaceJiliServiceBalanceRequest request, ServerCallContext context)
    {
        var result = await _balanceService.GetAsync(new BalanceQuery(
             ApiUrl: new(request.ApiUrl),
             AgentId: request.AgentId,
             AgentKey: request.AgentKey,
             PlayerCode: request.PlayerCode)).ConfigureAwait(false);

        return new GameInterfaceJiliServiceBalanceReply()
        {
            Success = result.Success,
            Message = result.Message ?? String.Empty,
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance
        };
    }

    public override async Task<GameInterfaceJiliServiceGamePullRecordReply> GetGamePullRecord(GameInterfaceJiliServiceGamePullRecordRequest request, ServerCallContext context)
    {
        var result = await _gameRecordService.QueryAsync(new GameRecordQuery(
            ApiUrl: new(request.ApiUrl),
            AgentId: request.AgentId,
            AgentKey: request.AgentKey,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset(),
            Page: request.Page,
            PageLimit: request.PageLimit)).ToArrayAsync().ConfigureAwait(false);

        var reply = new GameInterfaceJiliServiceGamePullRecordReply();

        if (result is null)
        {
            return reply;
        }

        foreach (var transaction in result)
        {
            reply.Items.Add(new GameInterfaceJiliServiceGamePullRecord()
            {
                Account = transaction.Account,
                WagersId = transaction.WagersId,
                GameId = transaction.GameId,
                WagersTime = transaction.WagersTime.ToTimestamp(),
                BetAmount = transaction.BetAmount,
                PayoffTime = transaction.PayoffTime.ToTimestamp(),
                PayoffAmount = transaction.PayoffAmount,
                Status = (GameInterfaceJiliServiceGamePullRecordStatus)transaction.Status,
                SettlementTime = transaction.SettlementTime.ToTimestamp(),
                GameCategoryId = transaction.GameCategoryId,
                Type = transaction.Type,
                AgentId = transaction.AgentId
            });
        };

        return reply;
    }

    public override async Task<GameInterfaceJiliServiceGameUrlReply> GetGameUrl(GameInterfaceJiliServiceGameUrlRequest request, ServerCallContext context)
    {
        var result = await _gameUrlService.GetAsync(new GameUrlQuery(
             ApiUrl: new(request.ApiUrl),
             AgentId: request.AgentId,
             AgentKey: request.AgentKey,
             PlayerCode: request.PlayerCode,
             ExternalGameId: request.ExternalGameId,
             Language: request.Language)).ConfigureAwait(false);

        return new GameInterfaceJiliServiceGameUrlReply()
        {
            Success = result.Success,
            Message = result.Message ?? String.Empty,
            IsPlayerExist = result.IsPlayerExist,
            GameUrl = result.GameUrl
        };
    }

    public override async Task<GameInterfaceJiliServiceReply> KickPlayer(GameInterfaceJiliServiceKickPlayerRequest request, ServerCallContext context)
    {
        await _kickMemberService.ExecuteAsync(new KickMemberCommand(
               ApiUrl: new(request.ApiUrl),
               AgentId: request.AgentId,
               AgentKey: request.AgentKey,
               PlayerCode: request.PlayerCode)).ConfigureAwait(false);

        return new GameInterfaceJiliServiceReply()
        {
            Success = true
        };
    }
}
