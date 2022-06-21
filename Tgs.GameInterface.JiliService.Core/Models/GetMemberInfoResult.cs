namespace Tgs.GameInterface.JiliService.Core.Models
{
    public record GetMemberInfoResult(
        bool Success,
        string? Message = null,
        bool IsPlayerExist = false,
        decimal? Balance = null);
}
