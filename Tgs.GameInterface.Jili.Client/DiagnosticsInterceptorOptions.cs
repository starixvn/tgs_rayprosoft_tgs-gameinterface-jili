using System.Diagnostics;

namespace Tgs.GameInterface.Jili.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.JiliService.Client.DiagnosticsInterceptor");
}
