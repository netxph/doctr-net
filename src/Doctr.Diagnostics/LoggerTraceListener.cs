using System.Diagnostics;
using System.Reflection;
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
                    method.DeclaringType == typeof(Debug) ||
                    method.DeclaringType.Name.Equals("TraceProvider") ||
                    method.DeclaringType.Name.Equals("TraceInternal"))  
            {
                depth++;
                method = new StackTrace().GetFrame(depth).GetMethod();
            }

            return method;
        }

    }

}
