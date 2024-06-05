namespace Tgs.GameInterface.Jili.Core.Models;

public record GetMemberInfoResult(
	bool IsPlayerExist,
	decimal Balance,
	bool Online);
