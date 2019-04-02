using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
namespace Aliyun.Serverless.Core.Http
{
    public abstract class FcHttpEntrypoint
    {
        /// <summary>
        /// Key to access the ILambdaContext object from the HttpContext.Items collection.
        /// </summary>
        public const string FC_CONTEXT = "FcContext";

        private FcHttpServer _server;

        protected IWebHost _host;

        private static string _pathBase;

        /// <summary>
        /// Should be called in the derived constructor 
        /// </summary>
        protected void Start()
        {
            var builder = CreateWebHostBuilder();
            Init(builder);

            _host = builder.Build();
            this.PostInit(_host);
            _host.Start();

            _server = _host.Services.GetService(typeof(Microsoft.AspNetCore.Hosting.Server.IServer)) as FcHttpServer;
            if (_server == null)
            {
                throw new Exception("Failed to find the implementation FcHttpServer for the IServer registration. This can happen if UseFcServer was not called.");
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IFcLogger Logger { get; private set; }

        public static string PathBase
        {
            get
            {
                return _pathBase;
            }
        }

        public static string AppCodePath
        {
            get; set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Aliyun.Serverless.Core.Http.FcHttpEntrypoint"/> is started.
        /// </summary>
        /// <value><c>true</c> if is started; otherwise, <c>false</c>.</value>
        private bool IsStarted
        {
            get
            {
                return _server != null;
            }
        }

        /// <summary>
        /// Method to initialize the web builder before starting the web host. In a typical Web API this is similar to the main function. 
        /// Setting the Startup class is required in this method.
        /// </summary>
        /// <example>
        /// <code>
        /// protected override void Init(IWebHostBuilder builder)
        /// {
        ///     builder
        ///         .UseStartup&lt;Startup&gt;();
        /// }
        /// </code>
        /// </example>
        /// <param name="builder"></param>
        protected abstract void Init(IWebHostBuilder builder);

        /// <summary>
        /// action after init.
        /// </summary>
        /// <param name="host">Host.</param>
        protected virtual void PostInit(IWebHost host) { }

        /// <summary>
        /// Creates the IWebHostBuilder similar to WebHost.CreateDefaultBuilder but replacing the registration of the Kestrel web server with a 
        /// registration for ApiGateway.
        /// </summary>
        /// <returns></returns>
        protected virtual IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    PhysicalFileProvider fileProvider = config.Properties["FileProvider"] as PhysicalFileProvider;
                    if (fileProvider != null)
                    {
                        Logger.LogInformation("FileProvider Root: {0}", fileProvider.Root);
                    }

                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                })
                .UseDefaultServiceProvider((hostingContext, options) =>
                {
                    options.ValidateScopes = hostingContext.HostingEnvironment.IsDevelopment();
                })
                .UseFcServer();

            return builder;
        }

        public virtual async Task<HttpResponse> HandleRequest(HttpRequest request, HttpResponse response, IFcContext fcContext)
        {
            Logger = fcContext.Logger;
            if (!IsStarted)
            {
                _pathBase = request.PathBase;
                Logger.LogInformation("Setting global PathBase {0}", _pathBase);
                Start();
            }

            Logger.LogDebug("Incoming {0} requests Path: {1}, Pathbase {2}", request.Method, request.Path, request.PathBase);

            InvokeFeatures features = new InvokeFeatures();
            MarshallRequest(features, request, fcContext);
            Logger.LogDebug($"ASP.NET Core Request PathBase: {((IHttpRequestFeature)features).PathBase}, Path: {((IHttpRequestFeature)features).Path}");

            var httpContext = this.CreateContext(features);

            if (request?.HttpContext?.User != null)
            {
                httpContext.HttpContext.User = request.HttpContext.User;
            }

            // Add along the Lambda objects to the HttpContext to give access to FC to them in the ASP.NET Core application
            httpContext.HttpContext.Items[FC_CONTEXT] = fcContext;

            // Allow the context to be customized before passing the request to ASP.NET Core.
            PostCreateContext(httpContext, request, fcContext);

            await this.ProcessRequest(fcContext, httpContext, features, response);

            return response;
        }

        /// <summary>
        /// This method is called after the FcHttpEntrypoint has marshalled the incoming API request
        /// into ASP.NET Core's IHttpRequestFeature. Derived classes can overwrite this method to alter
        /// the how the marshalling was done.
        /// </summary>
        /// <param name="aspNetCoreRequestFeature">ASP net core request feature.</param>
        /// <param name="request">Request.</param>
        /// <param name="fcContext">Fc context.</param>
        protected virtual void PostMarshallRequestFeature(IHttpRequestFeature aspNetCoreRequestFeature, HttpRequest request, IFcContext fcContext)
        {

        }


        /// <summary>
        /// This method is called after the FcHttpEntrypoint has marshalled the incoming API Gateway request
        /// into ASP.NET Core's IHttpConnectionFeature. Derived classes can overwrite this method to alter
        /// the how the marshalling was done.
        /// </summary>
        /// <param name="aspNetCoreConnectionFeature">ASP net core connection feature.</param>
        /// <param name="request">Request.</param>
        /// <param name="fcContext">Fc context.</param>
        protected virtual void PostMarshallConnectionFeature(IHttpConnectionFeature aspNetCoreConnectionFeature, HttpRequest request, IFcContext fcContext)
        {

        }


        /// <summary>
        /// This method is called after the FcHttpEntrypoint has marshalled IHttpResponseFeature that came
        /// back from making the request into ASP.NET Core into API Gateway's response object HttpResonse. Derived classes can overwrite this method to alter
        /// the how the marshalling was done.
        /// </summary>
        /// <param name="aspNetCoreResponseFeature">ASP net core response feature.</param>
        /// <param name="response">Response.</param>
        /// <param name="fcContext">Fc context.</param>
        protected virtual void PostMarshallResponseFeature(IHttpResponseFeature aspNetCoreResponseFeature, HttpResponse response, IFcContext fcContext)
        {

        }


        /// <summary>
        /// This method is called after the HostingApplication.Context has been created. Derived classes can overwrite this method to alter
        /// the context before passing the request to ASP.NET Core to process the request.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="request">Request.</param>
        /// <param name="fcContext">Fc context.</param>
        protected virtual void PostCreateContext(HostingApplication.Context context, HttpRequest request, IFcContext fcContext)
        {

        }

        /// <summary>
        /// Creates a <see cref="HostingApplication.Context"/> object using the <see cref="FcHttpServer"/> field in the class.
        /// </summary>
        /// <param name="features"><see cref="IFeatureCollection"/> implementation.</param>
        protected HostingApplication.Context CreateContext(IFeatureCollection features)
        {
            return _server.Application.CreateContext(features);
        }

        /// <summary>
        /// Convert the JSON document received from API Gateway into the InvokeFeatures object.
        /// InvokeFeatures is then passed into IHttpApplication to create the ASP.NET Core request objects.
        /// </summary>
        /// <param name="features">Features.</param>
        /// <param name="request">Request.</param>
        /// <param name="fcContext">Fc context.</param>
        protected void MarshallRequest(InvokeFeatures features, HttpRequest request, IFcContext fcContext)
        {
            {
                var requestFeatures = (IHttpRequestFeature)features;
                requestFeatures.Scheme = "https";
                requestFeatures.Method = request.Method;

                requestFeatures.Path = request.Path; // the PathString ensures it starts with "/";
                requestFeatures.PathBase = request.PathBase;
                requestFeatures.QueryString = request.QueryString.Value;
                requestFeatures.Headers = request.Headers;
                requestFeatures.Body = request.Body;

                // Call consumers customize method in case they want to change how API request
                // was marshalled into ASP.NET Core request.
                PostMarshallRequestFeature(requestFeatures, request, fcContext);
            }


            {
                // set up connection features
                var connectionFeatures = (IHttpConnectionFeature)features;
                connectionFeatures.RemoteIpAddress = request?.HttpContext?.Connection?.RemoteIpAddress;
                if (request?.HttpContext?.Connection?.RemotePort != null)
                {
                    connectionFeatures.RemotePort = (request?.HttpContext?.Connection?.RemotePort).Value;
                }

                if (request?.Headers?.ContainsKey("X-Forwarded-Port") == true)
                {
                    connectionFeatures.RemotePort = int.Parse(request.Headers["X-Forwarded-Port"]);
                }

                // Call consumers customize method in case they want to change how API Gateway's request
                // was marshalled into ASP.NET Core request.
                PostMarshallConnectionFeature(connectionFeatures, request, fcContext);
            }
        }

        /// <summary>
        /// Convert the response coming from ASP.NET Core into APIGatewayProxyResponse which is
        /// serialized into the JSON object that API Gateway expects.
        /// </summary>
        /// <returns>The response.</returns>
        /// <param name="responseFeatures">Response features.</param>
        /// <param name="fcContext">Fc context.</param>
        /// <param name="statusCodeIfNotSet">Status code if not set.</param>
        protected HttpResponse MarshallResponse(IHttpResponseFeature responseFeatures, IFcContext fcContext, HttpResponse response, int statusCodeIfNotSet = 200)
        {
            response.StatusCode = responseFeatures.StatusCode != 0 ? responseFeatures.StatusCode : statusCodeIfNotSet;
            string contentType = null;
            if (responseFeatures.Headers != null)
            {
                foreach (var kvp in responseFeatures.Headers)
                {
                    response.Headers[kvp.Key] = kvp.Value;

                    // Remember the Content-Type for possible later use
                    if (kvp.Key.Equals("Content-Type", StringComparison.CurrentCultureIgnoreCase))
                        contentType = response.Headers[kvp.Key];
                }
            }

            if (contentType == null)
            {
                response.Headers["Content-Type"] = StringValues.Empty;
            }

            response.Body = responseFeatures.Body;

            PostMarshallResponseFeature(responseFeatures, response, fcContext);

            return response;
        }

        /// <summary>
        /// Processes the current request.
        /// </summary>
        /// <param name="fcContext"><see cref="IFcContext"/> implementation.</param>
        /// <param name="context">The hosting application request context object.</param>
        /// <param name="features">An <see cref="InvokeFeatures"/> instance.</param>
        /// <param name="rethrowUnhandledError">
        /// If specified, an unhandled exception will be rethrown for custom error handling.
        /// Ensure that the error handling code calls 'this.MarshallResponse(features, 500);' after handling the error to return a <see cref="HttpResponse"/> to the user.
        /// </param>
        protected async Task<HttpResponse> ProcessRequest(IFcContext fcContext, HostingApplication.Context context, InvokeFeatures features, HttpResponse response, bool rethrowUnhandledError = false)
        {
            var defaultStatusCode = 200;
            Exception ex = null;
            try
            {
                await this._server.Application.ProcessRequestAsync(context);
            }
            catch (AggregateException agex)
            {
                ex = agex;
                Logger.LogError($"Caught AggregateException: '{agex}'");
                var sb = new StringBuilder();
                foreach (var newEx in agex.InnerExceptions)
                {
                    sb.AppendLine(this.ErrorReport(newEx));
                }

                Logger.LogError(sb.ToString());
                defaultStatusCode = 500;
            }
            catch (ReflectionTypeLoadException rex)
            {
                ex = rex;
                Logger.LogError($"Caught ReflectionTypeLoadException: '{rex}'");
                var sb = new StringBuilder();
                foreach (var loaderException in rex.LoaderExceptions)
                {
                    var fileNotFoundException = loaderException as FileNotFoundException;
                    if (fileNotFoundException != null && !string.IsNullOrEmpty(fileNotFoundException.FileName))
                    {
                        sb.AppendLine($"Missing file: {fileNotFoundException.FileName}");
                    }
                    else
                    {
                        sb.AppendLine(this.ErrorReport(loaderException));
                    }
                }

                Logger.LogError(sb.ToString());
                defaultStatusCode = 500;
            }
            catch (Exception e)
            {
                ex = e;
                if (rethrowUnhandledError) throw;
                Logger.LogError($"Unknown error responding to request: {this.ErrorReport(e)}");
                defaultStatusCode = 500;
            }
            finally
            {
                this._server.Application.DisposeContext(context, ex);
            }

            if (features.ResponseStartingEvents != null)
            {
                await features.ResponseStartingEvents.ExecuteAsync();
            }

            this.MarshallResponse(features, fcContext, response, defaultStatusCode);

            if (features.ResponseCompletedEvents != null)
            {
                await features.ResponseCompletedEvents.ExecuteAsync();
            }

            return response;
        }

        /// <summary>
        /// Formats an Exception into a string, including all inner exceptions.
        /// </summary>
        /// <param name="e"><see cref="Exception"/> instance.</param>
        protected string ErrorReport(Exception e)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{e.GetType().Name}:\n{e}");

            Exception inner = e;
            while (inner != null)
            {
                // Append the messages to the StringBuilder.
                sb.AppendLine($"{inner.GetType().Name}:\n{inner}");
                inner = inner.InnerException;
            }

            return sb.ToString();
        }
    }
}
