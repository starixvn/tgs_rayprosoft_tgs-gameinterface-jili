using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.Logging;

namespace Tgs.GameInterface.JiliService.Repositories.UnitTest
{
    [TestClass]
    public class CreateMemberUnitTest
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
            _ = handler.Expect(HttpMethod.Post, "/api1/CreateMember")
                .Respond(HttpStatusCode.OK, new StringContent(content));

            var fakeHttpClient = handler.ToHttpClient();
            var fakeLog = NullLogger<GameApiClient>.Instance;
            var fakeTgsLog = Substitute.For<ITgsLog>();

            var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

            await client.CreateMemberAsync(
                  apiUrl: apiUrl,
                  agentId: agentId,
                  agentKey: agentKey,
                  playerCode: playerCode).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Player_Already_Exists()
        {
            var apiUrl = new Uri("http://localhost/");
            var agentId = "testAgentId";
            var agentKey = "testAgentKey";
            var playerCode = "test";

            var response = new
            {
                ErrorCode = 101,
                Message = "Member Exists"
            };

            var content = JsonSerializer.Serialize(response);
            var handler = new MockHttpMessageHandler();
            _ = handler.Expect(HttpMethod.Post, "/api1/CreateMember")
                .Respond(HttpStatusCode.OK, new StringContent(content));

            var fakeHttpClient = handler.ToHttpClient();
            var fakeLog = NullLogger<GameApiClient>.Instance;
            var fakeTgsLog = Substitute.For<ITgsLog>();

            var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

            await client.CreateMemberAsync(
                  apiUrl: apiUrl,
                  agentId: agentId,
                  agentKey: agentKey,
                  playerCode: playerCode).ConfigureAwait(false);
        }
    }
}
