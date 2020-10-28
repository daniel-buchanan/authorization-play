using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Resources;
using authorization_play.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace authorization_play.Api
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
            services.AddDbContext<AuthorizationPlayContext>();
            services.AddSingleton<IResourceStorage, ResourceStorage>();
            services.AddSingleton<IPermissionGrantStorage, PermissionGrantStorage>();
            services.AddScoped<IResourceFinder, ResourceFinder>();
            services.AddScoped<IResourceValidator, ResourceValidator>();
            services.AddSingleton<IPermissionTicketStorage, PermissionTicketStorage>();
            services.AddScoped<IPermissionValidator, PermissionValidator>();
            services.AddScoped<IPermissionGrantFinder, PermissionGrantFinder>();
            services.AddScoped<IPermissionTicketManager, PermissionTicketManager>();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthorizationPlayContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            context.Database.Migrate();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
