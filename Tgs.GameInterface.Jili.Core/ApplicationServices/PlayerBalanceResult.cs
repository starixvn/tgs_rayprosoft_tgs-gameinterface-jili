namespace Tgs.GameInterface.Jili.Core.ApplicationServices;

public record PlayerBalanceResult(
	bool IsPlayerExist,
	decimal Balance,
	bool Online);
