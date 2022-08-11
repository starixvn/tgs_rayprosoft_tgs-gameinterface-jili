namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

public record KickMemberCommand(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode);
