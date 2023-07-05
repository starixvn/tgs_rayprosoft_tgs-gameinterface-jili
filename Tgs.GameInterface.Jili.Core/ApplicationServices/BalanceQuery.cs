namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record BalanceQuery(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode);
