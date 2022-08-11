namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

public record BalanceQuery(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode);
