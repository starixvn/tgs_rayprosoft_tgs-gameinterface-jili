using System.Text.Json;
using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Core.Models;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
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

		var query = new FundOutQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			PlayerCode: playerCode,
			TransactionId: transactionId,
			Amount: amount);

		var fakeClient = Substitute.For<IGameApiClient>();

		var result = new ExchangeTransferByAgentIdResult(
			TransactionId: transactionId,
			CoinBefore: 0,
			CoinAfter: 100,
			CurrencyBefore: 100,
			CurrencyAfter: 0,
			JsonSerializer.Serialize(new
			{
				TransactionId = transactionId,
				CoinBefore = 0m,
				CoinAfter = 10m,
				CurrencyBefore = 10m,
				CurrencyAfter = 10m,
				Status = 1
			}));
		_ = fakeClient.ExchangeTransferByAgentIdAsync(
			Arg.Is(apiUrl),
			Arg.Is(agentId),
			Arg.Is(agentKey),
			Arg.Is(playerCode),
			Arg.Is(transactionId),
			Arg.Is(TransferType.FundOut),
			Arg.Is(amount)).Returns(result);

		var service = new FundOutGetService(fakeClient);

		var actual = await service.GetAsync(query);

		Assert.Equal(result.TransactionId, actual.TransactionId);
	}
}
