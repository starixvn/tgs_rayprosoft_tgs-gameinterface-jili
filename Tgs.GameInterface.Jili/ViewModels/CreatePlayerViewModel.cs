namespace Tgs.GameInterface.Jili.ViewModels;

public record CreatePlayerViewModel(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode);
