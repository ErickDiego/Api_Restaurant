using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurant.Data;
using Restaurant.Repository.Contratos;
using Restaurant.Repository.Repositorios;

namespace Restaurant.WebApi
{
    public class Startup
    {
        private const string enUSCulture = "en-US";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:49845",
                                        "http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });// Make sure you call this previous to AddMvc
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<RestaurantBDContext>(options =>
                                                        options.UseSqlServer(Configuration.
                                                                        GetConnectionString("RestaurantBD")));
            services.AddScoped<IProductosRepositorio, ProductosRepositorio>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
