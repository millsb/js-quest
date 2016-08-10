using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using JsQuest.Models;

namespace JsQuest {
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env) 
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddUserSecrets()
                .AddEnvironmentVariables();

            Configuration = builder.Build();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) 
        {
            app.UseDeveloperExceptionPage();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => 
                builder.AllowAnyOrigin().AllowAnyHeader());
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)  
        {
            // Add framework services.
            services.AddMvc();
            services.AddCors();

            var connectionString = Configuration.GetConnectionString("development");
            services.AddDbContext<PlayerContext>(
                opts => opts.UseNpgsql(connectionString)
            );
        }
    }
}