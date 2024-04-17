namespace Tgs.GameInterface.Jili.ViewModels;

public record BalanceQueryViewModel(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string PlayerCode);
