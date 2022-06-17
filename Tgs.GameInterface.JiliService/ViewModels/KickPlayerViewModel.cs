namespace Tgs.GameInterface.JiliService.ViewModels
{
    public record KickPlayerViewModel(
        Uri ApiUrl,
        string AgentId,
        string AgentKey,
        string PlayerCode);
}
