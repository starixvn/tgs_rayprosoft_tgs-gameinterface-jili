using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Polly;
using Tgs.GameInterface.JiliService.Core.Models;

namespace Tgs.GameInterface.JiliService.Core;

internal class GameApiClient : IGameApiClient
{
    private static readonly JsonSerializerOptions? _SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    private readonly HttpClient _client;
    private readonly ILogger<GameApiClient> _logger;

    public GameApiClient(
        ILogger<GameApiClient> logger,
        HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<bool> CheckTransferByTransactionIdAsync(Uri apiUrl, string agentId, string agentKey, string transactionId, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["TransactionId"] = transactionId,
            ["AgentId"] = agentId
        };

        var keyGen = GetKeyGen(agentId, agentKey);
        var key = GetKey(keyGen, parameters);
        parameters["Key"] = key;

        var document = await PostAsync(
            apiUrl,
            "/api1/CheckTransferByTransactionId",
            parameters,
            "CheckTransferByTransactionId",
            cancellationToken).ConfigureAwait(false)
            ?? throw new Exception("Document is empty.");

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "0" and not "101")
        {
            throw new Exception($"This game manufacturer's transfer order number does not exist.({transactionId})");
        }

        if (errorCode is "101")
        {
            return false;
        }

        var data = document.RootElement.GetProperty("Data");

        var status = data.GetProperty("Status").GetInt32();

        return status == 1;
    }

    public async ValueTask CreateMemberAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["Account"] = playerCode,
            ["AgentId"] = agentId
        };

        var keyGen = GetKeyGen(agentId, agentKey);
        var key = GetKey(keyGen, parameters);
        parameters["Key"] = key;

        var document = await PostAsync(
                apiUrl: apiUrl,
                path: "/api1/CreateMember",
                parameters: parameters,
                logPrefix: "CreateMember",
                cancellationToken: cancellationToken).ConfigureAwait(false)
                ?? throw new Exception("Document is empty.");

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "0" and not "101")
        {
            throw new Exception($"Failed to create player.({playerCode})");
        }
    }

    public async ValueTask<ExchangeTransferByAgentIdResult> ExchangeTransferByAgentIdAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, TransferType transferType, decimal amount, CancellationToken cancellationToken = default)
    {
        amount *= 10000;
        amount = Math.Floor(amount);
        amount /= 10000;

        var parameters = new Dictionary<string, string>
        {
            ["Account"] = playerCode,
            ["TransactionId"] = transactionId,
            ["Amount"] = amount.ToString(),
            ["TransferType"] = ((int)transferType).ToString("d"),
            ["AgentId"] = agentId
        };

        var keyGen = GetKeyGen(agentId, agentKey);
        var key = GetKey(keyGen, parameters);
        parameters["Key"] = key;

        var tryCount = 3;
        var retryPolicy = Policy.HandleResult<JsonDocument?>(r =>
        {
            var errorCode = r?.RootElement.GetProperty("ErrorCode").ToString();
            return errorCode == "106";
        })
         .WaitAndRetryAsync(tryCount,
             retryAttempt => TimeSpan.FromSeconds(Random.Shared.Next(1, maxValue: 20)),
             (result, retryTime, context) =>
             {
                 _logger.LogWarning("LogOn:{logOn}  Parameter:{parameters} Result:{result} Delay:{retryTime}",
                     DateTime.Now.ToString("o"),
                     parameters,
                     result,
                     retryTime);
             });

        var document = await retryPolicy
            .ExecuteAsync(
                async () => await PostAsync(
                    apiUrl: apiUrl,
                    path: "/api1/ExchangeTransferByAgentId",
                    parameters: parameters,
                    logPrefix: "ExchangeTransferByAgentId",
                    cancellationToken: cancellationToken))
            .ConfigureAwait(false)
            ?? throw new Exception("Document is empty.");

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "0")
        {
            return new ExchangeTransferByAgentIdResult(
                Success: false,
                Message: document.RootElement.GetProperty("Message").ToString());
        }

        var data = document.RootElement.GetProperty("Data");

        if (!data.TryGetProperty("TransactionId", out var jsonGameProductTransactionId) ||
            !data.TryGetProperty("CoinBefore", out var jsonCoinBefore) ||
            !data.TryGetProperty("CoinAfter", out var jsonCoinAfter) ||
            !data.TryGetProperty("CurrencyBefore", out var jsonCurrencyBefore) ||
            !data.TryGetProperty("CurrencyAfter", out var jsonCurrencyAfter) ||
            !data.TryGetProperty("Status", out var jsonStatus))

        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }
        var success = jsonStatus.GetInt16() == 1;

        var coinBefore = default(decimal);
        var coinAfter = default(decimal);
        var currencyBefore = default(decimal);
        var CurrencyAfter = default(decimal);
        if (success)
        {
            _ = jsonCoinBefore.TryGetDecimal(out coinBefore);
            _ = jsonCoinAfter.TryGetDecimal(out coinAfter);
            _ = jsonCurrencyBefore.TryGetDecimal(out currencyBefore);
            _ = jsonCurrencyAfter.TryGetDecimal(out CurrencyAfter);
        }

        return new ExchangeTransferByAgentIdResult(
            Success: success,
            TransactionId: jsonGameProductTransactionId.ToString(),
            CoinBefore: coinBefore,
            CoinAfter: coinAfter,
            CurrencyBefore: currencyBefore,
            CurrencyAfter: CurrencyAfter);
    }

    public async IAsyncEnumerable<GetBetRecordTransaction> GetBetRecordByTimeAsync(
        Uri apiUrl,
        string agentId,
        string agentKey,
        DateTime startTime,
        DateTime endTime,
        int page = 1,
        int pageLimit = 100,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");

        startTime = TimeZoneInfo.ConvertTimeFromUtc(startTime, _timeZone);
        endTime = TimeZoneInfo.ConvertTimeFromUtc(endTime, _timeZone);
        var parameters = new Dictionary<string, string>
        {
            ["StartTime"] = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["EndTime"] = endTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["Page"] = page.ToString(),
            ["PageLimit"] = pageLimit.ToString(),
            ["AgentId"] = agentId
        };

        var keyGen = GetKeyGen(agentId, agentKey);
        var key = GetKey(keyGen, parameters);
        parameters["Key"] = key;
        parameters["FilterAgent"] = "1";

        var document = await GetAsync(
                apiUrl: apiUrl,
                path: "/api1/GetBetRecordByTime",
                parameters: parameters,
                logPrefix: "GetBetRecordByTime",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? throw new Exception("Document is empty.");

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "0")
        {
            throw new Exception($"Game provider error.({JsonSerializer.Serialize(document.RootElement.GetProperty("Message").ToString())})");
        }

        if (!document.RootElement.TryGetProperty("Data", out var jsonData))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        if (!jsonData.TryGetProperty("Result", out var jsonResult))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        if (string.IsNullOrWhiteSpace(jsonResult.ToString()))
        {
            yield break;
        }

        foreach (var transaction in jsonResult.EnumerateArray())
        {
            yield return new GetBetRecordTransaction()
            {
                Account = transaction.GetProperty("Account").ToString(),
                WagersId = transaction.GetProperty("WagersId").ToString(),
                GameId = transaction.GetProperty("GameId").GetInt32(),
                WagersTime = DateTimeOffset.Parse(transaction.GetProperty("WagersTime").ToString()),
                BetAmount = -transaction.GetProperty("BetAmount").GetDecimal(),
                PayoffTime = DateTimeOffset.Parse(transaction.GetProperty("PayoffTime").ToString()),
                PayoffAmount = transaction.GetProperty("PayoffAmount").GetDecimal(),
                Status = (JiLiStatus)transaction.GetProperty("Status").GetInt32(),
                SettlementTime = DateTimeOffset.Parse(transaction.GetProperty("SettlementTime").ToString()),
                GameCategoryId = transaction.GetProperty("GameCategoryId").GetInt32(),
                VersionKey = transaction.GetProperty("VersionKey").GetInt32(),
                Type = transaction.GetProperty("Type").GetInt32(),
                AgentId = transaction.GetProperty("AgentId").ToString()
            };
        }
    }

    public async ValueTask<GetMemberInfoResult> GetMemberInfoAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["Accounts"] = playerCode,
            ["AgentId"] = agentId
        };

        var keyGen = GetKeyGen(agentId, agentKey);
        var key = GetKey(keyGen, parameters);
        parameters["Key"] = key;

        var document = await PostAsync(
                apiUrl: apiUrl,
                path: "/api1/GetMemberInfo",
                parameters: parameters,
                logPrefix: "GetMemberInfo",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? throw new Exception("Document is empty.");

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "0")
        {
            return new GetMemberInfoResult(
                Success: false,
                Message: document.RootElement.GetProperty("Message").ToString());
        }

        var datas = document.RootElement.GetProperty(propertyName: "Data");

        var memberInfo = datas.EnumerateArray().FirstOrDefault();

        var status = memberInfo.GetProperty("Status").GetInt32();

        var balance = memberInfo.GetProperty("Balance").GetDecimal();

        return new GetMemberInfoResult(
            Success: true,
            IsPlayerExist: status is not 3,
            Balance: balance);
    }

    public async ValueTask KickMemberAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["Account"] = playerCode,
            ["AgentId"] = agentId
        };

        var keyGen = GetKeyGen(agentId, agentKey);
        var key = GetKey(keyGen, parameters);
        parameters["Key"] = key;
        _ = await PostAsync(
            apiUrl,
            "/api1/KickMember",
            parameters,
            "KickMember",
            cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask<LoginWithoutRedirectResult> LoginWithoutRedirectAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string gameId, string lang, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["Account"] = playerCode,
            ["GameId"] = gameId,
            ["Lang"] = lang,
            ["AgentId"] = agentId
        };

        var keyGen = GetKeyGen(agentId, agentKey);
        var key = GetKey(keyGen, parameters);
        parameters["Key"] = key;

        var document = await GetAsync(
                apiUrl: apiUrl,
                path: "/api1/LoginWithoutRedirect",
                parameters: parameters,
                logPrefix: "LoginWithoutRedirect",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? throw new Exception("Document is empty.");

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        return errorCode is not "0" and not "14"
            ? new LoginWithoutRedirectResult(
                Success: false,
                Message: document.RootElement.GetProperty("Message").ToString())
            : new LoginWithoutRedirectResult(
            Success: true,
            IsPlayerExist: errorCode.ToString() is not "14",
            GameUrl: document.RootElement.GetProperty("Data").ToString()
            );
    }

    public async IAsyncEnumerable<GetBetRecordTransaction> GetBetRecordByTimeAsync(
        Uri apiUrl,
        string agentId,
        string agentKey,
        int gameId,
        DateTime startTime,
        DateTime endTime,
        int page = 1,
        int pageLimit = 100,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");

        startTime = TimeZoneInfo.ConvertTimeFromUtc(startTime, _timeZone);
        endTime = TimeZoneInfo.ConvertTimeFromUtc(endTime, _timeZone);
        var parameters = new Dictionary<string, string>
        {
            ["StartTime"] = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["EndTime"] = endTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["Page"] = page.ToString(),
            ["PageLimit"] = pageLimit.ToString(),
            ["GameId"] = gameId.ToString(),
            ["AgentId"] = agentId
        };

        var keyParameters = new Dictionary<string, string>
        {
            ["StartTime"] = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["EndTime"] = endTime.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["Page"] = page.ToString(),
            ["PageLimit"] = pageLimit.ToString(),
            ["AgentId"] = agentId
        };

        var keyGen = GetKeyGen(agentId, agentKey);
        var key = GetKey(keyGen, keyParameters);
        parameters["Key"] = key;
        parameters["FilterAgent"] = "1";

        var document = await GetAsync(
                apiUrl: apiUrl,
                path: "/api1/GetBetRecordByTime",
                parameters: parameters,
                logPrefix: "GetBetRecordByTime",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? throw new Exception("Document is empty.");

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "0")
        {
            throw new Exception($"Game provider error.({JsonSerializer.Serialize(document.RootElement.GetProperty("Message").ToString())})");
        }

        if (!document.RootElement.TryGetProperty("Data", out var jsonData))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        if (!jsonData.TryGetProperty("Result", out var jsonResult))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        if (string.IsNullOrWhiteSpace(jsonResult.ToString()))
        {
            yield break;
        }

        foreach (var transaction in jsonResult.EnumerateArray())
        {
            yield return new GetBetRecordTransaction()
            {
                Account = transaction.GetProperty("Account").ToString(),
                WagersId = transaction.GetProperty("WagersId").ToString(),
                GameId = transaction.GetProperty("GameId").GetInt32(),
                WagersTime = DateTimeOffset.Parse(transaction.GetProperty("WagersTime").ToString()),
                BetAmount = -transaction.GetProperty("BetAmount").GetDecimal(),
                PayoffTime = DateTimeOffset.Parse(transaction.GetProperty("PayoffTime").ToString()),
                PayoffAmount = transaction.GetProperty("PayoffAmount").GetDecimal(),
                Status = (JiLiStatus)transaction.GetProperty("Status").GetInt32(),
                SettlementTime = DateTimeOffset.Parse(transaction.GetProperty("SettlementTime").ToString()),
                GameCategoryId = transaction.GetProperty("GameCategoryId").GetInt32(),
                VersionKey = transaction.GetProperty("VersionKey").GetInt32(),
                Type = transaction.GetProperty("Type").GetInt32(),
                AgentId = transaction.GetProperty("AgentId").ToString()
            };
        }
    }

    private static string GetKey(string keyGen, IEnumerable<KeyValuePair<string, string>> parameters)
    {
        var queryString = string.Join("&", parameters.Select(pair => $"{pair.Key}={pair.Value}"));
        var value = $"{queryString}{keyGen}";

        var hash = MD5Hash(value);

        return $"{Guid.NewGuid().ToString("N")[..6]}{hash}{Guid.NewGuid().ToString("N")[..6]}";
    }

    private static string GetKeyGen(string agentId, string agentKey)
    {
        var cuiabaTime = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");//(UTC-04:00) Cuiaba

        var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cuiabaTime).ToString("yyMMd");
        var value = $"{date}{agentId}{agentKey}";

        return MD5Hash(value);
    }

    private static string MD5Hash(string original)
    {
        var data = Encoding.UTF8.GetBytes(original);
        var hash = MD5.HashData(data);

        var sb = new StringBuilder();

        foreach (var t in hash)
        {
            _ = sb.Append(t.ToString("x2"));
        }

        return sb.ToString();
    }

    private async ValueTask<JsonDocument?> GetAsync(Uri apiUrl, string path, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var url = new Uri(apiUrl, path);
        var requestUri = QueryHelpers.AddQueryString(url.OriginalString, parameters.ToDictionary(x => x.Key, x => x.Value));

        var sw = Stopwatch.StartNew();

        var httpResponseMessage = await _client.GetAsync(requestUri, cancellationToken: cancellationToken).ConfigureAwait(false);

        sw.Stop();

        var responseMessage = default(string);

        var stream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        using (var sr = new StreamReader(stream))
        {
            responseMessage = await sr.ReadToEndAsync().ConfigureAwait(false);
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = string.Empty,
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonDocument.Parse(responseMessage);
    }

    private async ValueTask<JsonDocument?> PostAsync(Uri apiUrl, string path, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();
        var url = new Uri(apiUrl, path);

        HttpContent content = new FormUrlEncodedContent(parameters);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
        {
            CharSet = "UTF-8"
        };

        var httpResponseMessage = await _client.PostAsync(url, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync().ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonDocument.Parse(responseMessage);
    }
}
