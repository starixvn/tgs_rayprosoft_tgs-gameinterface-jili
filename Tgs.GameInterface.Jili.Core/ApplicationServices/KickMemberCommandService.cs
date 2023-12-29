using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class KickMemberCommandService(
    IGameApiClient client) : ICommandService<KickMemberCommand>
{
    public ValueTask ExecuteAsync(KickMemberCommand command, CancellationToken cancellationToken = default)
        => client.KickMemberAsync(
            apiUrl: command.ApiUrl,
            agentId: command.AgentId,
            agentKey: command.AgentKey,
            playerCode: command.PlayerCode,
            cancellationToken: cancellationToken);
}
