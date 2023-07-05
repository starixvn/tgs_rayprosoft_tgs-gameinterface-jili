namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record FundOrderCheckingQuery(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string TransactionId);
