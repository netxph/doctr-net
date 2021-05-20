using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Doctr.Diagnostics
{
    public static class TraceExtensions
    {

        public static void BindTrace(this IServiceProvider provider)
        {
             Trace.Listeners.Add(provider.GetRequiredService<LoggerTraceListener>());
        }

        public static IServiceCollection AddDoctr(this IServiceCollection services)
        {
            services.AddTransient<LoggerTraceListener>();

            return services;
        }

    }

}
