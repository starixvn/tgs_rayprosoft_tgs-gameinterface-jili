using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Jili.Core.UnitTest;

[TestClass]
public class KickMemberUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var agentId = "testAgentId";
		var agentKey = "testAgentKey";
		var playerCode = "test";

		var response = new
		{
			ErrorCode = 0,
			Message = "Success"
		};

		var content = JsonSerializer.Serialize(response);
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/api1/KickMember")
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		await client.KickMemberAsync(
			  apiUrl: apiUrl,
			  agentId: agentId,
			  agentKey: agentKey,
			  playerCode: playerCode).ConfigureAwait(false);
	}
}
