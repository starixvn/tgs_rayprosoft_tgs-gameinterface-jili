using System.Text.Json;
using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Core.Models;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var agentId = "testAgentId";
		var agentKey = "testAgentKey";
		var playerCode = "test";
		var transactionId = Guid.NewGuid().ToString("N");
		var amount = 100m;

		var query = new FundInQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			PlayerCode: playerCode,
			TransactionId: transactionId,
			Amount: amount);

		var fakeClient = Substitute.For<IGameApiClient>();

		var result = new ExchangeTransferByAgentIdResult(
			TransactionId: transactionId,
			CoinBefore: 100.0m,
			CoinAfter: 0.0m,
			CurrencyAfter: 100.0m,
			CurrencyBefore: 0.0m,
			Resp: JsonSerializer.Serialize(new
			{
				TransactionId = transactionId,
				CoinBefore = 100.0m,
				CoinAfter = 0m,
				CurrencyBefore = 100.0m,
				CurrencyAfter = 0.0m,
				Status = 1
			}));
		_ = fakeClient.ExchangeTransferByAgentIdAsync(
			Arg.Is(apiUrl),
			Arg.Is(agentId),
			Arg.Is(agentKey),
			Arg.Is(playerCode),
			Arg.Is(transactionId),
			Arg.Is(TransferType.FundIn),
			Arg.Is(amount)).Returns(ValueTask.FromResult(result));

		var service = new FundInGetService(fakeClient);

		var actual = await service.GetAsync(query);

		Assert.Equal(result.TransactionId, actual.TransactionId);
	}
}
