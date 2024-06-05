namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record PlayerLogoutCommand(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string PlayerCode);
