namespace Aliyun.Serverless.Core
{
    using System;
    using Microsoft.Extensions.Logging;
    /// <summary>
    /// Object that allows you to access useful information available within
    /// the AliFc execution environment.
    /// </summary>
    public interface IFcContext
    {
        /// <summary>
        /// The AliFc request ID associated with the request.
        /// This is the same ID returned to the client that called invoke().
        /// This ID is reused for retries on the same request.
        /// </summary>
        string RequestId { get; }

        /// <summary>
        /// Gets the function parameter interface.
        /// </summary>
        /// <value>The function parameter interface.</value>
        IFunctionParameter FunctionParam {get;}

        /// <summary>
        /// AliFc logger associated with the Context object.
        /// </summary>
        IFcLogger Logger { get; }

        /// <summary>
        /// Gets the credentials interface.
        /// </summary>
        /// <value>The credentials interface.</value>
        ICredentials Credentials {get;}

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        /// <value>The account identifier.</value>
        string AccountId { get; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>The region.</value>
        string Region { get; }

        /// <summary>
        /// Gets the service meta.
        /// </summary>
        /// <value>The service meta.</value>
        IServiceMeta ServiceMeta { get; }
    }
}