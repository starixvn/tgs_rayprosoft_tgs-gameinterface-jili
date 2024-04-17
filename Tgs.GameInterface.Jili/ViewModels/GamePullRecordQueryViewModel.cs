namespace Tgs.GameInterface.Jili.ViewModels;

public record GamePullRecordQueryViewModel(
	Uri ApiUrl,
	string AgentId,
	string AgentKey,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime,
	int Page = 1,
	int PageLimit = 10000);
