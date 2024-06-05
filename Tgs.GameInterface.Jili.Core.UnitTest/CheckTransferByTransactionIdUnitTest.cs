using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Jili.Core.UnitTest;

public class CheckTransferByTransactionIdUnitTest
{
	[Fact]
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

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var atual = await client.CheckTransferByTransactionIdAsync(
			  apiUrl: apiUrl,
			  agentId: agentId,
			  agentKey: agentKey,
			  transactionId: transactionId);

		Assert.True(atual);
	}
}
