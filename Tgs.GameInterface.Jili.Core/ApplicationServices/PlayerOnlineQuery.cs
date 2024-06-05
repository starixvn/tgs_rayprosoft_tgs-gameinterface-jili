namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record PlayerOnlineQuery(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string PlayerCode);
