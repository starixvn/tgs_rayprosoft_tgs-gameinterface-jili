namespace Tgs.GameInterface.Jili.ViewModels;

public record KickPlayerViewModel(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string PlayerCode);
