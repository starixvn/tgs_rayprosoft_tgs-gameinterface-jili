namespace Tgs.GameInterface.Jili.ViewModels;

public record BalanceResultViewModel(
	bool Success,
	string? Message,
	bool IsPlayerExist,
	decimal? Balance);
