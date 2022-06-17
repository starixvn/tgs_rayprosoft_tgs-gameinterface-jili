namespace Tgs.GameInterface.JiliService.Core.ApplicationServices
{
    public record FundOrderCheckingQuery(
        Uri ApiUrl,
        string AgentId,
        string AgentKey,
        string TransactionId);
}
