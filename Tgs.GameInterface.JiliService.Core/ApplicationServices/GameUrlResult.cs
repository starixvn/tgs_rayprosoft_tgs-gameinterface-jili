namespace Tgs.GameInterface.JiliService.Core.ApplicationServices
{
    public record GameUrlResult(
        bool Success,
        string? Message = null,
        bool IsPlayerExist = false,
        string? GameUrl = null);
}
