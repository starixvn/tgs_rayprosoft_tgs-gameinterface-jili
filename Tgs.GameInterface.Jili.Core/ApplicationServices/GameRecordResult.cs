namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record GameRecordResult(
	string Account,
	string WagersId,
	int GameId,
	DateTimeOffset WagersTime,
	decimal BetAmount,
	DateTimeOffset PayoffTime,
	decimal PayoffAmount,
	JiLiStatus Status,
	DateTimeOffset SettlementTime,
	int GameCategoryId,
	int Type,
	string AgentId,
	decimal Turnover,
	string AdditionalData);
