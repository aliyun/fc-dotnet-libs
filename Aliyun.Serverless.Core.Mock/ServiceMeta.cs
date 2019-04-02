using System;
namespace Aliyun.Serverless.Core.Mock
{
    public class ServiceMeta : IServiceMeta
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the log project.
        /// </summary>
        /// <value>The log project.</value>
        public string LogProject { get; set; }

        /// <summary>
        /// Gets the log store.
        /// </summary>
        /// <value>The log store.</value>
        public string LogStore { get; set; }

        /// <summary>
        /// Gets the qualifier.
        /// </summary>
        /// <value>The qualifier.</value>
        public string Qualifier { get; set; }

        /// <summary>
        /// Gets the version identifier.
        /// </summary>
        /// <value>The version identifier.</value>
        public string VersionId { get; set; }
    }
}
