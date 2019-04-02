using System;
using Aliyun.Serverless.Core;
namespace Aliyun.Serverless.Core.Mock
{
    public class Credentials : ICredentials
    {
        /// <summary>
        /// Gets the access key identifier.
        /// </summary>
        /// <value>The access key identifier.</value>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// Gets the access key secret.
        /// </summary>
        /// <value>The access key secret.</value>
        public string AccessKeySecret { get; set; }

        /// <summary>
        /// Gets the security token.
        /// </summary>
        /// <value>The security token.</value>
        public string SecurityToken { get; set; }
    }
}
