using System;
namespace Aliyun.Serverless.Core
{
    /// <summary>
    /// Service meta.
    /// </summary>
    public interface IServiceMeta
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the log project.
        /// </summary>
        /// <value>The log project.</value>
        string LogProject { get; }

        /// <summary>
        /// Gets the log store.
        /// </summary>
        /// <value>The log store.</value>
        string LogStore { get; }

        /// <summary>
        /// Gets the qualifier.
        /// </summary>
        /// <value>The qualifier.</value>
        string Qualifier { get; }

        /// <summary>
        /// Gets the version identifier.
        /// </summary>
        /// <value>The version identifier.</value>
        string VersionId { get; }
    }
}
