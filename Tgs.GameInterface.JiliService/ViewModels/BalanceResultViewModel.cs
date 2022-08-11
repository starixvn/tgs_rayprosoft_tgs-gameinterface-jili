namespace Tgs.GameInterface.JiliService.ViewModels;

public record BalanceResultViewModel(
    bool Success,
    string? Message,
    bool IsPlayerExist,
    decimal? Balance);
