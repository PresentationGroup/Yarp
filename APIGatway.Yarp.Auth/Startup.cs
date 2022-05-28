using Microsoft.AspNetCore.Authentication.Cookies;
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

namespace APIGatway.Yarp.Auth
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
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIGatway.Yarp.Auth", Version = "v1" });
            //});


            //.................................

            // Add the reverse proxy capability to the server
            var proxyBuilder = services.AddReverseProxy();
            // Initialize the reverse proxy from the "ReverseProxy" section of configuration
            proxyBuilder.LoadFromConfig(Configuration.GetSection("ReverseProxy"));



            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

            services.AddAuthorization(options =>
            {
                // Creates a policy called "myPolicy" that depends on having a claim "myCustomClaim" with the value "green".
                // See AccountController.Login method for where this claim is applied to the user identity
                // This policy can then be used by routes in the proxy, see "ClaimsAuthRoute" in appsettings.json
                options.AddPolicy("myPolicy", builder => builder
                    .RequireClaim("myCustomClaim", "green")
                    .RequireAuthenticatedUser());

                // The default policy is to require authentication, but no additional claims
                // Uncommenting the following would have no effect
                // options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                // FallbackPolicy is used for routes that do not specify a policy in config
                // Make all routes that do not specify a policy to be anonymous (this is the default).
                options.FallbackPolicy = null;
                // Or make all routes that do not specify a policy require some auth:
                // options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();            
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIGatway.Yarp.Auth v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapReverseProxy();
            });
        }
    }
}
