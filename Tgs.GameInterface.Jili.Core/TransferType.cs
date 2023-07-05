namespace Tgs.GameInterface.Jili.Core;

public enum TransferType
{
    /// <summary>
    /// All fund out
    /// 從 遊戲商 轉移額度到 平臺商 (不看 amount 值，全部轉出)
    /// </summary>
    AllFundOut = 1,

    /// <summary>
    /// The fund out
    /// 從 平臺商 轉移額度到 遊戲商
    /// Tgs->GP
    /// </summary>
    FundOut = 2,

    /// <summary>
    /// The fund in
    /// 從 遊戲商 轉移額度到 平臺商
    /// GP->Tgs
    /// </summary>
    FundIn = 3,
}
