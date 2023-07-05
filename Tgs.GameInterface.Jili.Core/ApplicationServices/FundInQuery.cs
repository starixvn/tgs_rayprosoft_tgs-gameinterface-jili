namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
