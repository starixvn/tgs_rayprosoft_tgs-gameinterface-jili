namespace Tgs.GameInterface.JiliService.Core.Models
{
    public record LoginWithoutRedirectResult(
        bool Success,
        string? Message = null,
        bool IsPlayerExist = false,
        string? GameUrl = null);
}
