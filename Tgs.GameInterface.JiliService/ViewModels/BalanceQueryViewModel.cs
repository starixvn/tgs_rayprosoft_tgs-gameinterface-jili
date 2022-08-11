namespace Tgs.GameInterface.JiliService.ViewModels;

public record BalanceQueryViewModel(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode);
