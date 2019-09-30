using CustomerApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace CustomerApi
{
    public class Startup
    {
        private readonly ILogger<Startup> logger;
        public IConfiguration Configuration { get; }

        public Startup(
            IConfiguration configuration,
            ILogger<Startup> logger)
        {
            this.Configuration = configuration;
            this.logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerDbContext>(options => 
            {
                options.UseInMemoryDatabase("Customer");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<ICustomerRepository, EfCustomerRepository>();
            services.AddScoped<ISettings, FileSettings>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new Info { Title = "Customer API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setup.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(HandleException);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer API v1");
                c.RoutePrefix = "";
            });

            app.UseMvc();
        }

        private void HandleException(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                string exceptionText = exceptionHandlerPathFeature?.Error?.ToString() ?? "";
                string requestText = $"{context.Request.Method} {context.Request.Path} {context.Request.QueryString}";
                logger.LogError($"Unhandled exception.\r\nEXCEPTION: {exceptionText}\r\nREQUEST: {requestText}");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(@"{ ""errorDescription"": ""Sorry, an error occurred while processing your request"" }");
            });
        }
    }
}
