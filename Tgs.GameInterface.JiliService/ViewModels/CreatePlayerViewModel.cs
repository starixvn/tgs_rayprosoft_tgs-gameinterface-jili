namespace Tgs.GameInterface.JiliService.ViewModels
{
    public record CreatePlayerViewModel(
        Uri ApiUrl,
        string AgentId,
        string AgentKey,
        string PlayerCode);
}
