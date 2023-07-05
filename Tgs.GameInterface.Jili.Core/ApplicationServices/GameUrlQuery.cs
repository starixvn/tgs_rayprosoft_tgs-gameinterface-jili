namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record GameUrlQuery(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string PlayerCode,
    string ExternalGameId,
    string Language);
