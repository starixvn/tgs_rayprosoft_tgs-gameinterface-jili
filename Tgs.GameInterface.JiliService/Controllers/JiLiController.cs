using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.JiliService.Core.ApplicationServices;
using Tgs.GameInterface.JiliService.Protos;
using Tgs.GameInterface.JiliService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.JiliService.Controllers
{
    [ApiController]
    [Route("JiLi")]
    public class JiLiController : ControllerBase
    {
        /// <summary>
        /// Creates the player asynchronous.
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
        [HttpPost("create")]
        public ValueTask CreatePlayerAsync(
                [FromServices] ICommandService<CreatePlayerCommand> service,
                CreatePlayerViewModel model)
                => service.ExecuteAsync(new CreatePlayerCommand(
                    ApiUrl: model.ApiUrl,
                    AgentId: model.AgentId,
                    AgentKey: model.AgentKey,
                    PlayerCode: model.PlayerCode));

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
        [HttpPost("fund-in")]
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
                result.Success,
                result.Message,
                result.TransactionId,
                result.CoinBefore,
                result.CoinAfter,
                result.CurrencyBefore,
                result.CurrencyAfter);
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
        [HttpPost("fundorderchecking")]
        public ValueTask<bool> FundOrderCheckingAsync(
            [FromServices] IGetService<FundOrderCheckingQuery, bool> service,
            FundOrderCheckingViewModel model)
                => service.GetAsync(new FundOrderCheckingQuery(
                    ApiUrl: model.ApiUrl,
                    AgentId: model.AgentId,
                    AgentKey: model.AgentKey,
                    TransactionId: model.TransactionId));

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
        [HttpPost("fund-out")]
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
                result.Success,
                result.Message,
                result.TransactionId,
                result.CoinBefore,
                result.CoinAfter,
                result.CurrencyBefore,
                result.CurrencyAfter);
        }

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
                Success: result.Success,
                Message: result.Message,
                IsPlayerExist: result.IsPlayerExist,
                GameUrl: result.GameUrl);
        }

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
        [HttpPost("balance")]
        public async ValueTask<BalanceResultViewModel> GetBalanceAsync(
            [FromServices] IGetService<BalanceQuery, BalanceResult> service,
            BalanceQueryViewModel model)
        {
            var result = await service.GetAsync(new BalanceQuery(
                ApiUrl: model.ApiUrl,
                AgentId: model.AgentId,
                AgentKey: model.AgentKey,
                PlayerCode: model.PlayerCode)).ConfigureAwait(false);

            return new BalanceResultViewModel(
                result.Success,
                result.Message,
                result.IsPlayerExist,
                result.Balance);
        }

        /// <summary>
        /// Gets the game pull record asynchronous.
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
        ///             "startTime": "2022-06-16T13:00:00.000Z",
        ///             "endTime": "2022-06-16T14:00:00.000Z",
        ///             "page": 1,
        ///             "pageLimit": 100
        ///         }
        ///
        /// </remarks>
        [HttpPost("game/pullrecord")]
        public async IAsyncEnumerable<GamePullRecordResultViewModel> GetGamePullRecordAsync(
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

            if (!result.Any())
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
                             Status: (GameInterfaceJiliServiceGamePullRecordStatus)transaction.Status,
                             SettlementTime: transaction.SettlementTime,
                             GameCategoryId: transaction.GameCategoryId,
                             Type: transaction.Type,
                             AgentId: transaction.AgentId);
            }
        }

        /// <summary>
        /// Kicks the player asynchronous.
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
        [HttpPost("kick")]
        public ValueTask KickPlayerAsync(
            [FromServices] ICommandService<KickMemberCommand> service,
            KickPlayerViewModel model)
        => service.ExecuteAsync(new KickMemberCommand(
            ApiUrl: model.ApiUrl,
            AgentId: model.AgentId,
            AgentKey: model.AgentKey,
            PlayerCode: model.PlayerCode));
    }
}
