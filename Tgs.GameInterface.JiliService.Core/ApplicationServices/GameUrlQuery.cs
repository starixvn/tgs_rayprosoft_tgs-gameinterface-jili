namespace Tgs.GameInterface.JiliService.Core.ApplicationServices
{
    public record GameUrlQuery(
        Uri ApiUrl,
        string AgentId,
        string AgentKey,
        string PlayerCode,
        string ExternalGameId,
        string Language);
}
