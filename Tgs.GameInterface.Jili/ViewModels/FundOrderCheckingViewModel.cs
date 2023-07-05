namespace Tgs.GameInterface.Jili.ViewModels;

public record FundOrderCheckingViewModel(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string TransactionId);
