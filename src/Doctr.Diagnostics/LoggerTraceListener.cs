using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Doctr.Diagnostics
{

    public class LoggerTraceListener : TraceListener
    {
        private readonly ILogger<LoggerTraceListener> logger;

        public LoggerTraceListener(ILogger<LoggerTraceListener> logger)
        {
            this.logger = logger;
        }

        public override void Write(string message)
        {
            var method = GetCallingMethod();

            logger.LogTrace($"[{method.DeclaringType.Name}::{method.Name}]: {message}");
        }

        public override void WriteLine(string message)
        {
            var method = GetCallingMethod();

            logger.LogTrace($"[{method.DeclaringType.Name}::{method.Name}]: {message}");
        }

        public override void WriteLine(string message, string category)
        {
            var method = GetCallingMethod();

            logger.LogTrace($"[{method.DeclaringType.Name}::{method.Name}]: {category}:{message}");
        }

        protected virtual MethodBase GetCallingMethod()
        {
            var depth = 3;

            var method = new StackTrace().GetFrame(depth).GetMethod();

            while(method.DeclaringType == typeof(LoggerTraceListener) ||
                    method.DeclaringType == typeof(Trace) ||
                    method.DeclaringType.Name.Equals("TraceInternal"))  
            {
                depth++;
                method = new StackTrace().GetFrame(depth).GetMethod();
            }

            return method;
        }

    }

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
