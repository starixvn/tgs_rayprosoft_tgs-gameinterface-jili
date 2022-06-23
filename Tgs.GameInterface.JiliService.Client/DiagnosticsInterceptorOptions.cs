using System.Diagnostics;

namespace Tgs.GameInterface.JiliService.Client
{
    public class DiagnosticsInterceptorOptions
    {
        public ActivitySource InterceptorActivitySource { get; set; } =
            new ActivitySource($"Tgs.GameInterface.JiliService.Client.DiagnosticsInterceptor");
    }
}
