namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record BalanceResult(
	bool Success,
	string? Message,
	bool IsPlayerExist,
	decimal? Balance);
