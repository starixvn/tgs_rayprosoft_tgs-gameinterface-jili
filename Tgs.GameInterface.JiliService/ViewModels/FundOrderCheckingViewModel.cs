namespace Tgs.GameInterface.JiliService.ViewModels;

public record FundOrderCheckingViewModel(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string TransactionId);
