using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Protos;
using Tgs.GameInterface.Jili.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.Controllers;

[ApiController]
[Route("JiLi")]
public class JiLiController : ControllerBase
{
	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "playerCode": "4ucmqplxb029",
	///             "transactionId": "8BF8BE2F096D33EBE7A3AF808E26692D",
	///             "amount": 100
	///         }
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public async ValueTask<FundResultViewModel> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundInResult> service,
		FundQueryViewModel model)
	{
		var result = await service.GetAsync(new FundInQuery(
			ApiUrl: model.ApiUrl,
			AgentId: model.AgentId,
			AgentKey: model.AgentKey,
			PlayerCode: model.PlayerCode,
			TransactionId: model.TransactionId,
			Amount: model.Amount)).ConfigureAwait(false);

		return new FundResultViewModel(
			result.TransactionId,
			result.CoinBefore,
			result.CoinAfter,
			result.CurrencyBefore,
			result.CurrencyAfter,
			result.Resp);
	}

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "playerCode": "4ucmqplxb029",
	///             "transactionId": "8BF8BE2F096D33EBE7A3AF808E26692C",
	///             "amount": 100
	///         }
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public async ValueTask<FundResultViewModel> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundOutResult> service,
		FundQueryViewModel model)
	{
		var result = await service.GetAsync(new FundOutQuery(
			ApiUrl: model.ApiUrl,
			AgentId: model.AgentId,
			AgentKey: model.AgentKey,
			PlayerCode: model.PlayerCode,
			TransactionId: model.TransactionId,
			Amount: model.Amount)).ConfigureAwait(false);

		return new FundResultViewModel(
			result.TransactionId,
			result.CoinBefore,
			result.CoinAfter,
			result.CurrencyBefore,
			result.CurrencyAfter,
			result.Resp);
	}

	/// <summary>
	/// Funds the order checking asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Head
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "transactionId": "8BF8BE2F096D33EBE7A3AF808E26692D"
	///         }
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundOrderCheckingAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundOrderCheckingViewModel model)
			=> service.GetAsync(new FundStatusQuery(
				ApiUrl: model.ApiUrl,
				AgentId: model.AgentId,
				AgentKey: model.AgentKey,
				TransactionId: model.TransactionId));

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "playerCode": "4ucmqplxb029",
	///             "externalGameId": "13",
	///             "language": "zh-CN"
	///         }
	///
	/// </remarks>
	[HttpPost("gameurl")]
	public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, GameUrlResult> service,
		GameUrlQueryViewModel model)
	{
		var result = await service.GetAsync(new GameUrlQuery(
			ApiUrl: model.ApiUrl,
			AgentId: model.AgentId,
			AgentKey: model.AgentKey,
			PlayerCode: model.PlayerCode,
			ExternalGameId: model.ExternalGameId,
			Language: model.Language
			)).ConfigureAwait(false);

		return new GameUrlResultViewModel(
			IsPlayerExist: result.IsPlayerExist,
			GameUrl: result.GameUrl);
	}

	/// <summary>
	/// Games the record asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "startTime": "2023-04-18T01:00:00.000Z",
	///             "endTime": "2023-04-18T02:00:00.000Z",
	///             "page": 1,
	///             "pageLimit": 10000
	///         }
	///
	/// </remarks>
	[HttpPost("game/record")]
	public async IAsyncEnumerable<GamePullRecordResultViewModel> GameRecordAsync(
		[FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
		GamePullRecordQueryViewModel model)
	{
		var result = await service.QueryAsync(new GameRecordQuery(
			ApiUrl: model.ApiUrl,
			AgentId: model.AgentId,
			AgentKey: model.AgentKey,
			StartTime: model.StartTime,
			EndTime: model.EndTime,
			Page: model.Page,
			PageLimit: model.PageLimit)).ToArrayAsync().ConfigureAwait(false);

		if (result.Length == 0)
		{
			yield break;
		}

		foreach (var transaction in result)
		{
			yield return new GamePullRecordResultViewModel(
				Account: transaction.Account,
				WagersId: transaction.WagersId,
				GameId: transaction.GameId,
				WagersTime: transaction.WagersTime,
				BetAmount: transaction.BetAmount,
				PayoffTime: transaction.PayoffTime,
				PayoffAmount: transaction.PayoffAmount,
				Status: (GameRecordStatus)transaction.Status,
				SettlementTime: transaction.SettlementTime,
				GameCategoryId: transaction.GameCategoryId,
				Type: transaction.Type,
				AgentId: transaction.AgentId,
				Turnover: transaction.Turnover,
				AdditionalData: transaction.AdditionalData);
		}
	}

	/// <summary>
	/// Games the record detail asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "wagersId": "1680840597057173115",
	///             "language": "zh-CN"
	///         }
	/// </remarks>
	[HttpPost("game/record/detail")]
	public ValueTask<string> GameRecordDetailAsync(
		[FromServices] IGetService<GameRecordDetailQuery, string> service,
		GameDetailQueryViewModel model)
	=> service.GetAsync(new GameRecordDetailQuery(
		ApiUrl: model.ApiUrl,
		AgentId: model.AgentId,
		AgentKey: model.AgentKey,
		WagersId: model.WagersId,
		Language: model.Language));

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public async ValueTask<BalanceResultViewModel> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		BalanceQueryViewModel model)
	{
		var result = await service.GetAsync(new PlayerBalanceQuery(
			ApiUrl: model.ApiUrl,
			AgentId: model.AgentId,
			AgentKey: model.AgentKey,
			PlayerCode: model.PlayerCode)).ConfigureAwait(false);

		return new BalanceResultViewModel(
			result.IsPlayerExist,
			result.Balance,
			result.Online);
	}

	/// <summary>
	/// Players the create asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/create", Name = "PlayerCreate")]
	public ValueTask PlayerCreateAsync(
		[FromServices] ICommandService<PlayerCreateCommand> service,
		CreatePlayerViewModel model)
		=> service.ExecuteAsync(new PlayerCreateCommand(
			ApiUrl: model.ApiUrl,
			AgentId: model.AgentId,
			AgentKey: model.AgentKey,
			PlayerCode: model.PlayerCode));

	/// <summary>
	/// Players the logout asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="model">The model.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		KickPlayerViewModel model)
		=> service.ExecuteAsync(new PlayerLogoutCommand(
			ApiUrl: model.ApiUrl,
			AgentId: model.AgentId,
			AgentKey: model.AgentKey,
			PlayerCode: model.PlayerCode));

	/// <summary>
	/// Players the online.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://uat-wb-api.jlfafafa2.com/",
	///             "agentId": "PG_GPI_CNY",
	///             "agentKey": "82af5971ae233eaa003b774be4079e7902ea7ff4",
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/online", Name = "PlayerOnline")]
	public ValueTask<bool> PlayerOnline(
		[FromServices] IGetService<PlayerOnlineQuery, bool> service,
		[FromBody] PlayerOnlineQuery query)
		=> service.GetAsync(query);
}
