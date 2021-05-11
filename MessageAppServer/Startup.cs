using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MessageAppServer.DAL;
using Microsoft.EntityFrameworkCore;
using MessageAppServer.Filters;
using MessageAppServer.Repository;

namespace MessageAppServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
            services.AddScoped<IMessageContext, MessageContext>();
            services.AddScoped<IMessageRepository, MessageRepository>();
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
            // add the custom basic authentication filter
            app.UseBasicAuthentication();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
