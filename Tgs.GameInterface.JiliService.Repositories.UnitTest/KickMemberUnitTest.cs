using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.Logging;

namespace Tgs.GameInterface.JiliService.Repositories.UnitTest;

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

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeTgsLog = Substitute.For<ITgsLog>();

        var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

        await client.KickMemberAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              playerCode: playerCode).ConfigureAwait(false);
    }
}
