namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record GameRecordQuery(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    int Page = 1,
    int PageLimit = 10000);
