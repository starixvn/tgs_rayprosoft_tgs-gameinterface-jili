using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> client.CreateMemberAsync(
			apiUrl: command.ApiUrl,
			agentId: command.AgentId,
			agentKey: command.AgentKey,
			playerCode: command.PlayerCode,
			cancellationToken: cancellationToken);
}
