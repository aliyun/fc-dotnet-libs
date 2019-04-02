namespace Aliyun.Serverless.Core
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AliFc runtime logger.
    /// </summary>
    public interface IFcLogger : ILogger
    {
        /// <summary>
        /// Gets or sets the minimal log level that is enabled.
        /// </summary>
        /// <value>The minimal log level.</value>
        LogLevel EnabledLogLevel { get; set;}
    }
}