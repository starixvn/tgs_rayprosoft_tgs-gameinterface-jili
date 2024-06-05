namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record FundOutResult(
	string TransactionId,
	decimal CoinBefore,
	decimal CoinAfter,
	decimal CurrencyBefore,
	decimal CurrencyAfter,
	string Resp);
