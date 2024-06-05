using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Jili.Core.UnitTest;

public class LoginWithoutRedirectUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://uat-wb-api.jlfafafa2.com/");
		var agentId = "PG_GPI_CNY";
		var agentKey = "82af5971ae233eaa003b774be4079e7902ea7ff4";
		var playerCode = "test";
		var gameId = "13";
		var lang = "zh-CN";
		var response = new
		{
			ErrorCode = 0,
			Message = "",
			Data = "https://www.google.com"
		};

		var content = JsonSerializer.Serialize(response);
		var handler = new MockHttpMessageHandler();
		_ = handler.When(HttpMethod.Get, "/api1/LoginWithoutRedirect")
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var atual = await client.LoginWithoutRedirectAsync(
			  apiUrl: apiUrl,
			  agentId: agentId,
			  agentKey: agentKey,
			  playerCode: playerCode,
			  gameId: gameId,
			  lang: lang);

		Assert.NotNull(atual);
		Assert.True(atual.IsPlayerExist);
	}

	[Fact]
	public async Task Player_Does_Not_Exist()
	{
		var apiUrl = new Uri("http://localhost/");
		var agentId = "testAgentId";
		var agentKey = "testAgentKey";
		var playerCode = "test";
		var gameId = "13";
		var lang = "zh-CN";
		var response = new
		{
			ErrorCode = 14,
			Message = "User not exist!",
			Data = null as object
		};

		var content = JsonSerializer.Serialize(response);
		var handler = new MockHttpMessageHandler();
		_ = handler.When(HttpMethod.Get, "/api1/LoginWithoutRedirect")
			.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var atual = await client.LoginWithoutRedirectAsync(
			  apiUrl: apiUrl,
			  agentId: agentId,
			  agentKey: agentKey,
			  playerCode: playerCode,
			  gameId: gameId,
			  lang: lang);

		Assert.NotNull(atual);
		Assert.False(atual.IsPlayerExist);
	}
}
