namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record FundInResult(
	string TransactionId,
	decimal CoinBefore,
	decimal CoinAfter,
	decimal CurrencyBefore,
	decimal CurrencyAfter,
	string Resp);
