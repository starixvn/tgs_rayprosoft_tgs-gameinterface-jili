using Tgs.GameInterface.JiliService.Protos;

namespace Tgs.GameInterface.JiliService.ViewModels;

public record GamePullRecordResultViewModel(
    string Account,
    string WagersId,
    int GameId,
    DateTimeOffset WagersTime,
    decimal BetAmount,
    DateTimeOffset PayoffTime,
    decimal PayoffAmount,
    GameInterfaceJiliServiceGamePullRecordStatus Status,
    DateTimeOffset SettlementTime,
    int GameCategoryId,
    int Type,
    string AgentId);
