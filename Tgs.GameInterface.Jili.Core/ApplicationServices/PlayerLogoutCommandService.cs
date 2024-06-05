using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class PlayerLogoutCommandService(
	IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
		=> client.KickMemberAsync(
			apiUrl: command.ApiUrl,
			agentId: command.AgentId,
			agentKey: command.AgentKey,
			playerCode: command.PlayerCode,
			cancellationToken: cancellationToken);
}
