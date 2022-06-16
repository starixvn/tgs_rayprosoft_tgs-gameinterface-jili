namespace Tgs.GameInterface.JiliService.Core.ApplicationServices
{
    public record FundInResult(
        bool Success,
        string? Message,
        string? TransactionId,
        decimal? CoinBefore = null,
        decimal? CoinAfter = null,
        decimal? CurrencyBefore = null,
        decimal? CurrencyAfter = null);
}
