using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.JiliService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.GrpcServices;

public class GameInterfaceJiliGrpcService : GameInterfaceJiliService.GameInterfaceJiliServiceBase
{
    private readonly ILogger<GameInterfaceJiliGrpcService> _logger;
    private readonly IGetService<BalanceQuery, BalanceResult> _balanceService;
    private readonly ICommandService<CreatePlayerCommand> _createPlayerService;
    private readonly IGetService<FundInQuery, FundInResult> _fundInService;
    private readonly IGetService<FundOrderCheckingQuery, bool> _fundOrderCheckingService;
    private readonly IGetService<FundOutQuery, FundOutResult> _fundOutService;
    private readonly IQueryService<GameRecordQuery, GameRecordResult> _gameRecordService;
    private readonly IGetService<GameUrlQuery, GameUrlResult> _gameUrlService;
    private readonly ICommandService<KickMemberCommand> _kickMemberService;
    private readonly IGetService<GameDetailQuery, string> _gameDetailService;

    public GameInterfaceJiliGrpcService(
        ILogger<GameInterfaceJiliGrpcService> logger,
         IGetService<BalanceQuery, BalanceResult> balanceService,
         ICommandService<CreatePlayerCommand> createPlayerService,
         IGetService<FundInQuery, FundInResult> fundInService,
         IGetService<FundOrderCheckingQuery, bool> fundOrderCheckingService,
         IGetService<FundOutQuery, FundOutResult> fundOutService,
         IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
         IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
         ICommandService<KickMemberCommand> kickMemberService,
         IGetService<GameDetailQuery, string> gameDetailService
         )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _balanceService = balanceService ?? throw new ArgumentNullException(nameof(balanceService));
        _createPlayerService = createPlayerService ?? throw new ArgumentNullException(nameof(createPlayerService));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOrderCheckingService = fundOrderCheckingService ?? throw new ArgumentNullException(nameof(fundOrderCheckingService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _gameRecordService = gameRecordService ?? throw new ArgumentNullException(nameof(gameRecordService));
        _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
        _kickMemberService = kickMemberService ?? throw new ArgumentNullException(nameof(kickMemberService));
        _gameDetailService = gameDetailService ?? throw new ArgumentNullException(nameof(gameDetailService));
    }

    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<BaseReply> CreatePlayer(CreatePlayerRequest request, ServerCallContext context)
    {
        await _createPlayerService.ExecuteAsync(new CreatePlayerCommand(
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
        var result = await _fundInService.GetAsync(new FundInQuery(
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
        var result = await _fundOrderCheckingService.GetAsync(new FundOrderCheckingQuery(
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
        var result = await _fundOutService.GetAsync(new FundOutQuery(
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
        var result = await _balanceService.GetAsync(new BalanceQuery(
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
        var result = await _gameDetailService
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
        var result = await _gameRecordService.QueryAsync(new GameRecordQuery(
            ApiUrl: new(request.ApiUrl),
            AgentId: request.AgentId,
            AgentKey: request.AgentKey,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset(),
            Page: request.Page,
            PageLimit: request.PageLimit)).ToArrayAsync().ConfigureAwait(false);

        var reply = new GamePullRecordReply();

        if (result is null)
        {
            return reply;
        }

        foreach (var transaction in result)
        {
            reply.Items.Add(new GamePullRecord()
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
            });
        };

        return reply;
    }

    public override async Task<GameUrlReply> GetGameUrl(GameUrlRequest request, ServerCallContext context)
    {
        var result = await _gameUrlService.GetAsync(new GameUrlQuery(
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
        await _kickMemberService.ExecuteAsync(new KickMemberCommand(
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
