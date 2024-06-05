namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record PlayerCreateCommand(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string PlayerCode);
