using Tgs.GameInterface.Jili.Protos;

namespace Tgs.GameInterface.Jili.ViewModels;

public record GamePullRecordResultViewModel(
	string Account,
	string WagersId,
	int GameId,
	DateTimeOffset WagersTime,
	decimal BetAmount,
	DateTimeOffset PayoffTime,
	decimal PayoffAmount,
	GameRecordStatus Status,
	DateTimeOffset SettlementTime,
	int GameCategoryId,
	int Type,
	string AgentId,
	decimal Turnover,
	string AdditionalData);
