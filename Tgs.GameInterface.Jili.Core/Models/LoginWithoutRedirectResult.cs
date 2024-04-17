namespace Tgs.GameInterface.Jili.Core.Models;

public record LoginWithoutRedirectResult(
	bool Success,
	string? Message = null,
	bool IsPlayerExist = false,
	string? GameUrl = null);
