using Tgs.GameInterface.JiliService.Protos;

namespace Tgs.GameInterface.Jili.ViewModels;

public record GamePullRecordResultViewModel(
    string Account,
    string WagersId,
    int GameId,
    DateTimeOffset WagersTime,
    decimal BetAmount,
    DateTimeOffset PayoffTime,
    decimal PayoffAmount,
    GamePullRecordStatus Status,
    DateTimeOffset SettlementTime,
    int GameCategoryId,
    int Type,
    string AgentId,
    decimal Turnover,
    string AdditionalData);
