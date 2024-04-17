namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record FundOutQuery(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string PlayerCode,
	string TransactionId,
	decimal Amount);
