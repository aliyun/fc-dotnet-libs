namespace Aliyun.Serverless.Core
{
    using System;

    /// <summary>
    /// Function parameter.
    /// </summary>
    public interface IFunctionParameter
    {
        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        /// <value>The name of the function.</value>
        string FunctionName {get;}

        /// <summary>
        /// Gets the function handler.
        /// </summary>
        /// <value>The function handler.</value>
        string FunctionHandler {get;}


        /// <summary>
        /// Gets the initializer.
        /// </summary>
        /// <value>The initializer.</value>
        string FunctionInitializer { get; }

        /// <summary>
        /// Memory limit, in MB, you configured for the AliFc function.
        /// </summary>
        int MemoryLimitInMB { get; }

        /// <summary>
        /// Remaining execution time till the function will be terminated.
        /// At the time you create the AliFc function you set maximum time
        /// limit, at which time AliFc will terminate the function 
        /// execution.
        /// Information about the remaining time of function execution can be
        /// used to specify function behavior when nearing the timeout.
        /// </summary>
        TimeSpan FunctionTimeout { get; }

        /// <summary>
        /// Gets the initializer timeout.
        /// </summary>
        /// <value>The initializer timeout.</value>
        TimeSpan InitializationTimeout { get; }
    }
}