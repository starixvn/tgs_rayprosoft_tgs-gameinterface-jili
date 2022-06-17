using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Core.ApplicationServices
{
    internal class CreatePlayerCommandService : ICommandService<CreatePlayerCommand>
    {
        private readonly IGameApiClient _client;

        public CreatePlayerCommandService(IGameApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public ValueTask ExecuteAsync(CreatePlayerCommand command, CancellationToken cancellationToken = default)
            => _client.CreateMemberAsync(
                apiUrl: command.ApiUrl,
                agentId: command.AgentId,
                agentKey: command.AgentKey,
                playerCode: command.PlayerCode,
                cancellationToken: cancellationToken);
    }
}
