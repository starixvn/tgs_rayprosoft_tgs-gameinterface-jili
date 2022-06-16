using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.Logging;

namespace Tgs.GameInterface.JiliService.Repositories.UnitTest
{
    [TestClass]
    public class CheckTransferByTransactionIdUnitTest
    {
        [TestMethod]
        public async Task Normal_Process()
        {
            var apiUrl = new Uri("http://localhost/");
            var agentId = "testAgentId";
            var agentKey = "testAgentKey";
            var transactionId = Guid.NewGuid().ToString("N");

            var response = new
            {
                ErrorCode = "0",
                Message = string.Empty,
                Data = new
                {
                    Status = 1
                }
            };

            var content = JsonSerializer.Serialize(response);
            var handler = new MockHttpMessageHandler();
            _ = handler.Expect(HttpMethod.Post, "/api1/CheckTransferByTransactionId")
                    .Respond(HttpStatusCode.OK, new StringContent(content));

            var fakeHttpClient = handler.ToHttpClient();
            var fakeLog = NullLogger<GameApiClient>.Instance;
            var fakeTgsLog = Substitute.For<ITgsLog>();

            var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

            var atual = await client.CheckTransferByTransactionIdAsync(
                  apiUrl: apiUrl,
                  agentId: agentId,
                  agentKey: agentKey,
                  transactionId: transactionId).ConfigureAwait(false);

            Assert.IsNotNull(atual);
            Assert.AreEqual(true, atual);
        }
    }
}
