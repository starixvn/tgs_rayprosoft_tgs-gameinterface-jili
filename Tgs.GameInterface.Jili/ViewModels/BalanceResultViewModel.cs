namespace Tgs.GameInterface.Jili.ViewModels;

public record BalanceResultViewModel(
	bool IsPlayerExist,
	decimal Balance,
	bool Online);
