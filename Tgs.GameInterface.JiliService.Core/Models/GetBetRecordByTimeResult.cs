namespace Tgs.GameInterface.JiliService.Core.Models
{
    public record GetBetRecordByTimeResult(
        bool Success,
        string? Message = null,
        IEnumerable<GetBetRecordTransaction>? Transactions = null);
}
