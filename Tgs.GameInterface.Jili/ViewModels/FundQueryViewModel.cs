namespace Tgs.GameInterface.Jili.ViewModels;

public record FundQueryViewModel(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
