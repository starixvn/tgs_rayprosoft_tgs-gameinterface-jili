namespace Tgs.GameInterface.JiliService.Core.ApplicationServices
{
    public record CreatePlayerCommand(
        Uri ApiUrl,
        string AgentId,
        string AgentKey,
        string PlayerCode);
}
