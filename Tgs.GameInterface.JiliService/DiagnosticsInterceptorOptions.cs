using System.Diagnostics;

namespace Tgs.GameInterface.JiLiService;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.JiliService.DiagnosticsInterceptor");
}
