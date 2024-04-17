namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record KickMemberCommand(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string PlayerCode);
