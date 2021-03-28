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
using MessageAppServer.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin;
using Owin;
using MessageAppServer.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MessageAppServer.FIlters;
using Microsoft.AspNetCore.Authentication;

namespace MessageAppServer
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
            services.AddSignalR();
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials();
            }));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MessageAppServer", Version = "v1" });
            });
            services.AddDbContext<MessageContext>(options => options.UseSqlite(@"Data Source=C:\Users\44785\source\repos\MessageAppServer\MessageAppServer\MessageApp.db").UseLazyLoadingProxies());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MessageAppServer v1"));
            }

            app.UseHttpsRedirection();
            // add the custom basic authentication
            app.UseBasicAuthentication();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/messageHub");
            });
        }
    }
}
