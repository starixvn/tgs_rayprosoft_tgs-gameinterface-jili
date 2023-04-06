using System.Diagnostics;

namespace Tgs.GameInterface.JiliService;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.JiliService.DiagnosticsInterceptor");
}
