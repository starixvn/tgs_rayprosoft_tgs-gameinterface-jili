using System.Runtime.CompilerServices;
using Tgs.GameInterface.JiliService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

internal class GameRecordGetService : IQueryService<GameRecordQuery, GameRecordResult>
{
    private readonly IGameApiClient _client;

    public GameRecordGetService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordResult> QueryAsync(GameRecordQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var transactions = await _client
            .GetBetRecordByTimeAsync(
                apiUrl: query.ApiUrl,
                agentId: query.AgentId,
                agentKey: query.AgentKey,
                startTime: query.StartTime.UtcDateTime,
                endTime: query.EndTime.UtcDateTime,
                page: query.Page,
                pageLimit: query.PageLimit,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GetBetRecordTransaction>();

        if (!transactions.Any())
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
                AgentId: transaction.AgentId,
                Turnover: transaction.Turnover,
                AdditionalData: transaction.AdditionalData);
        }
    }
}
