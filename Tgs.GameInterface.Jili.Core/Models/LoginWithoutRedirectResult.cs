namespace Tgs.GameInterface.Jili.Core.Models;

public record LoginWithoutRedirectResult(
	bool IsPlayerExist,
	string GameUrl);
