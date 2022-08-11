using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.Logging;

namespace Tgs.GameInterface.JiliService.Repositories.UnitTest;

[TestClass]
public class GetMemberInfoUnitTest
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
            Message = string.Empty,
            Data = new[]{
                new{
                    Account = playerCode,
                    Balance = 100m,
                    Status = 1
                }
            }
        };

        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/api1/GetMemberInfo")
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeTgsLog = Substitute.For<ITgsLog>();

        var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

        var atual = await client.GetMemberInfoAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              playerCode: playerCode).ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(true, atual.Success);
        Assert.AreEqual(true, atual.IsPlayerExist);
    }

    [TestMethod]
    public async Task Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";

        var response = new
        {
            ErrorCode = 0,
            Message = string.Empty,
            Data = new[]{
                new{
                    Account = playerCode,
                    Balance = 0,
                    Status = 3
                }
            }
        };

        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/api1/GetMemberInfo")
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeTgsLog = Substitute.For<ITgsLog>();

        var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

        var atual = await client.GetMemberInfoAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              playerCode: playerCode).ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(true, atual.Success);
        Assert.AreEqual(false, atual.IsPlayerExist);
    }
}
