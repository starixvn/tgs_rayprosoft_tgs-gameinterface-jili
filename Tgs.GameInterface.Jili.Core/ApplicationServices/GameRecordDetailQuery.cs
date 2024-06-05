namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record GameRecordDetailQuery(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	string WagersId,
	string Language);
