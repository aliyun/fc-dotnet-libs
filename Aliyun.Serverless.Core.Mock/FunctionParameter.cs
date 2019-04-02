using System;
using Aliyun.Serverless.Core;
namespace Aliyun.Serverless.Core.Mock
{
    public class FunctionParameter : IFunctionParameter
    {
        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        /// <value>The name of the function.</value>
        public string FunctionName { get; set; }

        /// <summary>
        /// Gets the function handler.
        /// </summary>
        /// <value>The function handler.</value>
        public string FunctionHandler { get; set; }


        /// <summary>
        /// Gets the initializer.
        /// </summary>
        /// <value>The initializer.</value>
        public string FunctionInitializer { get; set; }

        /// <summary>
        /// Memory limit, in MB, you configured for the AliFc function.
        /// </summary>
        public int MemoryLimitInMB { get; set; }

        /// <summary>
        /// Remaining execution time till the function will be terminated.
        /// At the time you create the AliFc function you set maximum time
        /// limit, at which time AliFc will terminate the function 
        /// execution.
        /// Information about the remaining time of function execution can be
        /// used to specify function behavior when nearing the timeout.
        /// </summary>
        public TimeSpan FunctionTimeout { get; set; }

        /// <summary>
        /// Gets the initializer timeout.
        /// </summary>
        /// <value>The initializer timeout.</value>
        public TimeSpan InitializationTimeout { get; set; }
    }
}
