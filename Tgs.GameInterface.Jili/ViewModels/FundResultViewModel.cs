namespace Tgs.GameInterface.Jili.ViewModels;

public record FundResultViewModel(
	string TransactionId,
	decimal CoinBefore,
	decimal CoinAfter,
	decimal CurrencyBefore,
	decimal CurrencyAfter,
	string Resp);
