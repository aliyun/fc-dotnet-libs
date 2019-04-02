using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server;
using Aliyun.Serverless.Core.Http;

namespace Microsoft.AspNetCore.Hosting
{
    public static class FcWebHostBuilderExtensions
    {
        public static IWebHostBuilder UseFcServer(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices(services =>
            {
                var serviceDescription = services.FirstOrDefault(x => x.ServiceType == typeof(IServer));
                if (serviceDescription != null)
                {
                    // If Fc server has already been added the skip out.
                    if (serviceDescription.ImplementationType == typeof(FcHttpServer))
                        return;
                    // If there is already an IServer registered then remove it. This is mostly likely caused
                    // by leaving the UseKestrel call.
                    else
                        services.Remove(serviceDescription);
                }

                services.AddSingleton<IServer, FcHttpServer>();
            });
        }
    }
}