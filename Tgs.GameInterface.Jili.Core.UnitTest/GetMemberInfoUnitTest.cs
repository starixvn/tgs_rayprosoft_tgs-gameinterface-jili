using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Jili.Core.UnitTest;

public class GetMemberInfoUnitTest
{
	[Fact]
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

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var atual = await client.GetMemberInfoAsync(
			  apiUrl: apiUrl,
			  agentId: agentId,
			  agentKey: agentKey,
			  playerCode: playerCode);

		Assert.NotNull(atual);
		Assert.True(atual.IsPlayerExist);
		Assert.Equal(100.0m, atual.Balance);
		Assert.True(atual.Online);
	}

	[Fact]
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

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var atual = await client.GetMemberInfoAsync(
			  apiUrl: apiUrl,
			  agentId: agentId,
			  agentKey: agentKey,
			  playerCode: playerCode);

		Assert.NotNull(atual);
		Assert.False(atual.IsPlayerExist);
		Assert.False(atual.Online);
	}
}
