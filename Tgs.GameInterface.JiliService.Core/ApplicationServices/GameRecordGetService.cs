using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

internal class GameRecordGetService : IQueryService<GameRecordQuery, GameRecordResult>
{
    private readonly IGameApiClient _client;
    private readonly ILogger<GameRecordGetService> _logger;

    public GameRecordGetService(
        ILogger<GameRecordGetService> logger,
        IGameApiClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordResult> QueryAsync(GameRecordQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var transactions = await _client.GetBetRecordByTimeAsync(
            apiUrl: query.ApiUrl,
            agentId: query.AgentId,
            agentKey: query.AgentKey,
            startTime: query.StartTime.UtcDateTime,
            endTime: query.EndTime.UtcDateTime,
            page: query.Page,
            pageLimit: query.PageLimit,
            cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Time:{startTime}~{endTime} Page:{page} PageLimit:{pageLimit} 總筆數:{count}",
            query.StartTime.UtcDateTime,
             query.EndTime.UtcDateTime,
             query.Page,
             query.PageLimit,
             transactions.Count());

        if (transactions is null || !transactions.Any())
        {
            yield break;
        }

        foreach (var transaction in transactions)
        {
            yield return new GameRecordResult(
                         Account: transaction.Account,
                         WagersId: transaction.WagersId,
                         GameId: transaction.GameId,
                         WagersTime: transaction.WagersTime,
                         BetAmount: transaction.BetAmount,
                         PayoffTime: transaction.PayoffTime,
                         PayoffAmount: transaction.PayoffAmount,
                         Status: transaction.Status,
                         SettlementTime: transaction.SettlementTime,
                         GameCategoryId: transaction.GameCategoryId,
                         Type: transaction.Type,
                         AgentId: transaction.AgentId);
        }
    }
}
