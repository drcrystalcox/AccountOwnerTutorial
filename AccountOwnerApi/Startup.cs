using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AccountOwnerApi.Extensions;
using System.IO;
using Microsoft.AspNetCore.HttpOverrides;
using AutoMapper;
using Microsoft.OpenApi.Models;
using BusinessLogic;
using Repository;

namespace AccountOwnerApi
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
            services.ConfigureCors();
            services.ConfigureIISIntegration();
        
            services.ConfigureMySqlContext(Configuration);
/*  var connectionString = config["mysqlconnection:connectionString"];
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString), ServiceLifetime.Singleton);
        
*/


            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSingleton<IOwnerService, OwnerService>();
            services.AddSingleton<IOwnerRepository,OwnerRepository>();
            services.AddSingleton<IRepositoryWrapper,RepositoryWrapper>();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
	            c.SwaggerDoc("v1", new OpenApiInfo()
	            {
		            Version = "1"
	            });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
	            c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountOwner API V1");
	            c.RoutePrefix = string.Empty;
            });


            app.UseHttpsRedirection();

            app.UseStaticFiles();
            
                app.UseCors("CorsPolicy");
            
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.All
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
