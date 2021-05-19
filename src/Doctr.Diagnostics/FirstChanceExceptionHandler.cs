using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Text;

namespace Doctr.Diagnostics
{

    public class FirstChanceExceptionHandler : IDisposable
    {

        public FirstChanceExceptionHandler()
        {
            AppDomain.CurrentDomain.FirstChanceException += OnHandleException;
        }

        protected virtual void OnHandleException(object source, FirstChanceExceptionEventArgs e)
        {
            Trace.WriteLine(OnBuildException(e.Exception));
        }

        protected virtual string OnBuildException(Exception ex)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"[{ex.GetType().Name}] {ex.Message}\r\nStackTrace:{ex.StackTrace}");

            var inner = ex.InnerException;

            if(inner != null)
            {
                builder.AppendLine(OnBuildException(inner));
            }

            return builder.ToString();
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.FirstChanceException -= OnHandleException;
        }
    }
}
