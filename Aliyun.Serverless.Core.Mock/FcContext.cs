using System;
using Aliyun.Serverless.Core;

namespace Aliyun.Serverless.Core.Mock
{
    public class FcContext : IFcContext
    {
        private FunctionParameter _functionParam = new FunctionParameter();
        private ConsoleLogger _logger = new ConsoleLogger(Microsoft.Extensions.Logging.LogLevel.Information);
        private ICredentials _credentials = new Credentials();
        private ServiceMeta _meta = new ServiceMeta();
        public FcContext(string accountId, string reqId)
        {
            AccountId = accountId;
            this.RequestId = reqId;
        }

        /// <summary>
        /// The AliFc request ID associated with the request.
        /// This is the same ID returned to the client that called invoke().
        /// This ID is reused for retries on the same request.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets the function parameter interface.
        /// </summary>
        /// <value>The function parameter interface.</value>
        public IFunctionParameter FunctionParam { get { return _functionParam; } }

        /// <summary>
        /// AliFc logger associated with the Context object.
        /// </summary>
        public IFcLogger Logger { get { return _logger; } }

        /// <summary>
        /// Gets the credentials interface.
        /// </summary>
        /// <value>The credentials interface.</value>
        public ICredentials Credentials { get { return _credentials; } set { _credentials = value; } }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        /// <value>The account identifier.</value>
        public string AccountId { get; set; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>The region.</value>
        public string Region { get; }

        /// <summary>
        /// Gets the service meta.
        /// </summary>
        /// <value>The service meta.</value>
        public IServiceMeta ServiceMeta { get { return _meta; } }
    }
}
