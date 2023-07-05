namespace Tgs.GameInterface.Jili.Core.Models;

public record GetBetRecordTransaction
{
    /// <summary>
    /// The account
    /// </summary>
    public string Account { get; set; } = default!;
    /// <summary>
    /// The wagers identifier
    /// </summary>
    public string WagersId { get; set; } = default!;
    /// <summary>
    /// The game identifier
    /// </summary>
    public int GameId { get; set; } = default!;
    /// <summary>
    /// The wagers time
    /// </summary>
    public DateTimeOffset WagersTime { get; set; } = default!;
    /// <summary>
    /// The bet amount
    /// </summary>
    public decimal BetAmount { get; set; }
    /// <summary>
    /// The payoff time
    /// </summary>
    public DateTimeOffset PayoffTime { get; set; } = default!;
    /// <summary>
    /// The payoff amount
    /// </summary>
    public decimal PayoffAmount { get; set; }
    /// <summary>
    /// The status
    /// </summary>
    public JiLiStatus Status { get; set; }
    /// <summary>
    /// The settlement time
    /// </summary>
    public DateTimeOffset SettlementTime { get; set; } = default!;
    /// <summary>
    /// The game category identifier
    /// </summary>
    public int GameCategoryId { get; set; }
    /// <summary>
    /// The version key
    /// </summary>
    public int VersionKey { get; set; }
    /// <summary>
    /// The type
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// The agent identifier
    /// </summary>
    public string AgentId { get; set; } = default!;

    /// <summary>
    /// The turnover
    /// </summary>
    public decimal Turnover { get; set; }

    public string AdditionalData { get; set; } = default!;
}
