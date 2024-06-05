namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string TransactionId);
