using APIGatway.Yarp.Auth.Metric.Mids;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarp.Telemetry.Consumption;

namespace APIGatway.Yarp.Auth.Metric
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIGatway.Yarp.Auth.Metric", Version = "v1" });
            });


            //.................................

            // Add the reverse proxy capability to the server
            var proxyBuilder = services.AddReverseProxy();
            // Initialize the reverse proxy from the "ReverseProxy" section of configuration
            proxyBuilder.LoadFromConfig(Configuration.GetSection("ReverseProxy"));



            services.AddHttpContextAccessor();

            // Interface that collects general metrics about the proxy forwarder
            services.AddSingleton<IMetricsConsumer<ForwarderMetrics>, ForwarderMetricsConsumer>();

            // Registration of a consumer to events for proxy forwarder telemetry
            services.AddTelemetryConsumer<ForwarderTelemetryConsumer>();

            // Registration of a consumer to events for HttpClient telemetry
            services.AddTelemetryConsumer<HttpClientTelemetryConsumer>();

            services.AddTelemetryConsumer<WebSocketsTelemetryConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


   
            // Custom middleware that collects and reports the proxy metrics
            // Placed at the beginning so it is the first and last thing run for each request
            app.UsePerRequestMetricCollection();

            // Middleware used to intercept the WebSocket connection and collect telemetry exposed to WebSocketsTelemetryConsumer
            app.UseWebSocketsTelemetry();

            app.UseRouting();

            app.UseCors();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy();
            });
        }
    }
}
