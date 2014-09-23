using System;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;
using NLog.Mvc;

namespace TaskBook.WebApi.Tracer
{
    /// <summary>
    /// Trace writer; uses NLog
    /// </summary>
    public sealed class NLogTracer: ITraceWriter
    {
        private readonly ILogger _logger;

        public NLogTracer(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Invokes trace action, logs the message record
        /// </summary>
        /// <param name="request">The current System.Net.Http.HttpRequestMessage</param>
        /// <param name="category">The logical category for the trace</param>
        /// <param name="level">TraceLevel at which to write this trace</param>
        /// <param name="traceAction">The action to invoke</param>
        public void Trace(HttpRequestMessage request,
            string category,
            TraceLevel level,
            Action<TraceRecord> traceAction)
        {
            if(level != TraceLevel.Off)
            {
                var record = new TraceRecord(request, category, level);
                traceAction(record);
                Log(record);
            }
        }

        private void Log(TraceRecord record)
        {
            var message = new StringBuilder();

            if(record.Request != null)
            {
                if(record.Request.Method != null)
                    message.Append(record.Request.Method);

                if(record.Request.RequestUri != null)
                    message.Append(" ").Append(record.Request.RequestUri);
            }

            if(!string.IsNullOrWhiteSpace(record.Category))
                message.Append(" ").Append(record.Category);

            if(!string.IsNullOrWhiteSpace(record.Operator))
                message.Append(" ").Append(record.Operator).Append(" ").Append(record.Operation);

            if(!string.IsNullOrWhiteSpace(record.Message))
                message.Append(" ").Append(record.Message);

            if(record.Exception != null && !string.IsNullOrWhiteSpace(record.Exception.GetBaseException().Message))
                message.Append(" ").Append(record.Exception.GetBaseException().Message);

            var logAction = GetTraceAction(record.Level);
            logAction(message.ToString());
        }

        private Action<string> GetTraceAction(TraceLevel level)
        {
            Action<string> action = null;
            switch(level)
            {
                case TraceLevel.Off:
                    // Do nothing
                    break;
                case TraceLevel.Debug:
                    action = _logger.Debug;
                    break;
                case TraceLevel.Error:
                case TraceLevel.Fatal:
                    action = _logger.Error;
                    break;
                case TraceLevel.Info:
                    action = _logger.Information;
                    break;
                case TraceLevel.Warn:
                    action = _logger.Warning;
                    break;
                default:
                    action = _logger.Trace;
                    break;
            }
            return action;
        }
    }
}