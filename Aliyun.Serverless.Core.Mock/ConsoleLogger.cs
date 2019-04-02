using System;
using Aliyun.Serverless.Core;
using Microsoft.Extensions.Logging;
namespace Aliyun.Serverless.Core.Mock
{
    public class ConsoleLogger : IFcLogger
    {
        public ConsoleLogger(LogLevel logLevel)
        {
            EnabledLogLevel = logLevel;
        }

        /// <summary>
        /// Gets or sets the request Id.
        /// </summary>
        /// <value>The prefix.</value>
        public string RequestId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the enabled log level.
        /// </summary>
        /// <value>The enabled log level.</value>
        public LogLevel EnabledLogLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                string s = formatter(state, exception);
                Console.WriteLine(string.Format("{0} {1} [{2}] {3}", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), RequestId, ToFcLogLevel(logLevel), s));
            }
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= EnabledLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// convert LogLevel to the fc log level.
        /// </summary>
        /// <returns>The fc log level.</returns>
        /// <param name="logLevel">Log level.</param>
        public static string ToFcLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    return "INFO";
                case LogLevel.Trace:
                    return "TRACE";
                case LogLevel.Debug:
                    return "DEBUG";
                case LogLevel.Warning:
                    return "WARNING";
                case LogLevel.Error:
                case LogLevel.Critical:
                    return "ERROR";
            }

            return "UNKNOWN";
        }

        /// <summary>
        /// Tos the log level.
        /// </summary>
        /// <returns>The log level.</returns>
        /// <param name="fcLogLevel">Fc log level.</param>
        public static LogLevel ToLogLevel(string fcLogLevel)
        {
            if (fcLogLevel == null)
            {
                return LogLevel.Information;
            }

            switch (fcLogLevel.ToUpper())
            {
                case "INFO":
                    return LogLevel.Information;
                case "TRACE":
                    return LogLevel.Trace;
                case "DEBUG":
                    return LogLevel.Debug;
                case "WARNING":
                    return LogLevel.Warning;
                case "ERROR":
                    return LogLevel.Error;
                default:
                    break;
            }

            return LogLevel.Information; // default
        }
    }
}
