namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

public record BalanceResult(
    bool Success,
    string? Message,
    bool IsPlayerExist,
    decimal? Balance);
