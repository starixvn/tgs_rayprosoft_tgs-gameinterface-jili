namespace Tgs.GameInterface.Jili.Core.Models;

public record ExchangeTransferByAgentIdResult(
	string TransactionId,
	decimal CoinBefore,
	decimal CoinAfter,
	decimal CurrencyBefore,
	decimal CurrencyAfter,
	string Resp);
