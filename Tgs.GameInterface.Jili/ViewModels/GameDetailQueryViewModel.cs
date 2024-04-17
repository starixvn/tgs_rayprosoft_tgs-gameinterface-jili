namespace Tgs.GameInterface.Jili.ViewModels;

public record GameDetailQueryViewModel(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string WagersId,
	string Language);
