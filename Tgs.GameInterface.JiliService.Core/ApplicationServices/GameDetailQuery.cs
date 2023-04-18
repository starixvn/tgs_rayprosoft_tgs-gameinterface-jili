namespace Tgs.GameInterface.JiliService.Core.ApplicationServices;

public record GameDetailQuery(
    Uri ApiUrl,
    string AgentId,
    string AgentKey,
    string WagersId,
    string Language);
