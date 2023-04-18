namespace Tgs.GameInterface.JiliService.ViewModels;

public record GameDetailQueryViewModel(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string WagersId,
    string Language);
