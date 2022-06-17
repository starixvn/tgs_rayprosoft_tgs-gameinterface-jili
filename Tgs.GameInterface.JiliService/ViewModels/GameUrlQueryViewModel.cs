namespace Tgs.GameInterface.JiliService.ViewModels
{
    public record GameUrlQueryViewModel(
        Uri ApiUrl,
        string AgentId,
        string AgentKey,
        string PlayerCode,
        string ExternalGameId,
        string Language);
}
