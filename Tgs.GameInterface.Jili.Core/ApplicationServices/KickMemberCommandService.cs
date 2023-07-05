using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class KickMemberCommandService : ICommandService<KickMemberCommand>
{
    private readonly IGameApiClient _client;

    public KickMemberCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(KickMemberCommand command, CancellationToken cancellationToken = default)
        => _client.KickMemberAsync(
            apiUrl: command.ApiUrl,
            agentId: command.AgentId,
            agentKey: command.AgentKey,
            playerCode: command.PlayerCode,
            cancellationToken: cancellationToken);
}
