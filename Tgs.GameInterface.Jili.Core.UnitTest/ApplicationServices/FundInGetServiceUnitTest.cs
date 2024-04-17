using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Core.Models;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

[TestClass]
public class FundInGetServiceUnitTest
{
	[TestMethod]
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
			Success: true,
			TransactionId: transactionId,
			CoinBefore: 100,
			CoinAfter: 0,
			CurrencyAfter: 100,
			CurrencyBefore: 0);
		_ = fakeClient.ExchangeTransferByAgentIdAsync(
			Arg.Is(apiUrl),
			Arg.Is(agentId),
			Arg.Is(agentKey),
			Arg.Is(playerCode),
			Arg.Is(transactionId),
			Arg.Is(TransferType.FundIn),
			Arg.Is(amount)).Returns(ValueTask.FromResult(result));

		var service = new FundInGetService(fakeClient);

		var actual = await service.GetAsync(query).ConfigureAwait(false);

		Assert.IsTrue(actual.Success);
		Assert.AreEqual(result.TransactionId, actual.TransactionId);
	}
}
