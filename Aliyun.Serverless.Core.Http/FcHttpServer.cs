using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;

namespace Aliyun.Serverless.Core.Http
{
    public class FcHttpServer : IServer
    {
        /// <summary>
        /// The application is used by the Fc function to initiate a web request through the ASP.NET Core framework.
        /// </summary>
        public IHttpApplication<HostingApplication.Context> Application { get; set; }

        /// <summary>
        /// Gets the features.
        /// </summary>
        /// <value>The features.</value>
        public IFeatureCollection Features { get; } = new FeatureCollection();

        public void Dispose()
        {
        }

        public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
        {
            this.Application = application as IHttpApplication<HostingApplication.Context>;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
