namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string PlayerCode);
