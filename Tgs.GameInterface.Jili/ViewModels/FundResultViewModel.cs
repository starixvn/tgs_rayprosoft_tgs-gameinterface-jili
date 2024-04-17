namespace Tgs.GameInterface.Jili.ViewModels;

public record FundResultViewModel(
	bool Success,
	string? Message,
	string? TransactionId,
	decimal? CoinBefore = null,
	decimal? CoinAfter = null,
	decimal? CurrencyBefore = null,
	decimal? CurrencyAfter = null);
