using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class CreatePlayerCommandService(IGameApiClient client) : ICommandService<CreatePlayerCommand>
{
	public ValueTask ExecuteAsync(CreatePlayerCommand command, CancellationToken cancellationToken = default)
		=> client.CreateMemberAsync(
			apiUrl: command.ApiUrl,
			agentId: command.AgentId,
			agentKey: command.AgentKey,
			playerCode: command.PlayerCode,
			cancellationToken: cancellationToken);
}
