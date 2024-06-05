using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class PlayerOnlineGetService(IGameApiClient client) : IGetService<PlayerOnlineQuery, bool>
{
	public async ValueTask<bool> GetAsync(PlayerOnlineQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.GetMemberInfoAsync(
			query.ApiUrl,
			query.AgentId,
			query.AgentKey,
			query.PlayerCode,
			cancellationToken).ConfigureAwait(false);

		return result.Online;
	}
}
