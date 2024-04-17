using System.Diagnostics;

namespace Tgs.GameInterface.Jili;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.Jili.DiagnosticsInterceptor");
}
