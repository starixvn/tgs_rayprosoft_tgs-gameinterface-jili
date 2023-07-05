namespace Tgs.GameInterface.Jili.Core.Models;

public record ExchangeTransferByAgentIdResult(
    bool Success,
    string? Message = null,
    string? TransactionId = null,
    decimal? CoinBefore = null,
    decimal? CoinAfter = null,
    decimal? CurrencyBefore = null,
    decimal? CurrencyAfter = null);
