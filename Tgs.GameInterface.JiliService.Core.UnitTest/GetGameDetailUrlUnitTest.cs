using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.JiliService.Core.UnitTest;

[TestClass]
public class GetGameDetailUrlUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://uat-wb-api.jlfafafa2.com/");
        var agentId = "PG_GPI_CNY";
        var agentKey = "82af5971ae233eaa003b774be4079e7902ea7ff4";
        var wagersId = "1680840597057173115";
        var lang = "zh-CN";
        var response = new
        {
            ErrorCode = 0,
            Message = "",
            Data = new
            {
                Url = "https://www.google.com"
            }
        };

        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.When(HttpMethod.Post, "/api1/GetGameDetailUrl")
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var atual = await client
            .GetGameDetailUrlAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              wagersId: wagersId,
              lang: lang)
            .ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(response.Data.Url, atual);
    }
}
