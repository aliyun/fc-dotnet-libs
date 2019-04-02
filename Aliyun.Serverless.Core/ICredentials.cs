namespace Aliyun.Serverless.Core
{
    /// <summary>
    /// Credentials interface
    /// </summary>
    public interface ICredentials
    {
        /// <summary>
        /// Gets the access key identifier.
        /// </summary>
        /// <value>The access key identifier.</value>
        string AccessKeyId {get;}

        /// <summary>
        /// Gets the access key secret.
        /// </summary>
        /// <value>The access key secret.</value>
        string AccessKeySecret {get;}

        /// <summary>
        /// Gets the security token.
        /// </summary>
        /// <value>The security token.</value>
        string SecurityToken {get;}
    }
}