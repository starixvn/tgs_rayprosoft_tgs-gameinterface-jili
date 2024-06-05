using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

internal class GameRecordDetailGetService(
	IGameApiClient client) : IGetService<GameRecordDetailQuery, string>
{
	public ValueTask<string> GetAsync(GameRecordDetailQuery query, CancellationToken cancellationToken = default)
		=> client
		.GetGameDetailUrlAsync(
			apiUrl: query.ApiUrl,
			agentId: query.AgentId,
			agentKey: query.AgentKey,
			wagersId: query.WagersId,
			lang: query.Language,
			cancellationToken: cancellationToken);
}
