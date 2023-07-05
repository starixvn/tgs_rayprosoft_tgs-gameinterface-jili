namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record CreatePlayerCommand(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode);
