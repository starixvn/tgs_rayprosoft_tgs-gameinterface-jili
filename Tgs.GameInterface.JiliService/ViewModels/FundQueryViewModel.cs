namespace Tgs.GameInterface.JiliService.ViewModels;

public record FundQueryViewModel(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
