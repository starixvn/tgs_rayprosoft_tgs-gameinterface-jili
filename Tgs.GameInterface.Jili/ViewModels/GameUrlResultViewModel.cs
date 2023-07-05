namespace Tgs.GameInterface.Jili.ViewModels;

public record GameUrlResultViewModel(
    bool Success,
    string? Message = null,
    bool IsPlayerExist = false,
    string? GameUrl = null);
