using NSubstitute;
using Tgs.GameInterface.JiliService.Core.ApplicationServices;
using Tgs.GameInterface.JiliService.Core.Models;

namespace Tgs.GameInterface.JiliService.Core.UnitTest.ApplicationServices;

[TestClass]
public class FundOutGetServiceUnitTest
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

        var query = new FundOutQuery(
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
            CoinBefore: 0,
            CoinAfter: 100,
            CurrencyAfter: 0,
            CurrencyBefore: 100);
        _ = fakeClient.ExchangeTransferByAgentIdAsync(
            Arg.Is(apiUrl),
            Arg.Is(agentId),
            Arg.Is(agentKey),
            Arg.Is(playerCode),
            Arg.Is(transactionId),
            Arg.Is(TransferType.FundOut),
            Arg.Is(amount)).Returns(result);

        var service = new FundOutGetService(fakeClient);

        var actual = await service.GetAsync(query).ConfigureAwait(false);

        Assert.IsTrue(actual.Success);
        Assert.AreEqual(result.TransactionId, actual.TransactionId);
    }
}
