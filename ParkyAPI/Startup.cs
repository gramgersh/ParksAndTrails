using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyAPI.Data;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkyAPI
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // This registers the Repository, which is the DB interface to various tables.
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();

            services.AddAutoMapper(typeof(ParkyMappings));
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;

            });
            // The GroupNameFormat is a literal 'v', plus VVV == version number.
            // So the controller class is Foov15Controller, where Foo is the controller,
            // v is the the literal character, and 15 is the VVV version number.
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            // This is the swagger API documentation.
//            services.AddSwaggerGen(options =>
//            {
//                options.SwaggerDoc("ParkyOpenAPISpec",
//                  new Microsoft.OpenApi.Models.OpenApiInfo()
//                  {
//                      Title = "Parky API",
//                      Version = "1.0.0",
//                      Description = "Udemy Parky AMI"
//                  });
//                //options.SwaggerDoc("ParkyOpenAPISpecTrails",
//                //  new Microsoft.OpenApi.Models.OpenApiInfo()
//                //  {
//                //      Title = "Parky API (Trails)",
//                //      Version = "1.0.0",
//                //      Description = "Udemy Parky AMI"
//                //  });
//                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
//                options.IncludeXmlComments(xmlCommentsFullPath);
//            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Temp turned off, as we have a self-signed cert.
            // app.UseHttpsRedirection();

            // Set upt he documentation uri and make it the default.
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                foreach ( var desc in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpperInvariant());
                options.RoutePrefix = "";

            });
            //            app.UseSwaggerUI(options => {
            //                options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
            //                //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
            //                // Make this the default index page for the site.
            //                options.RoutePrefix = "";
            //            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
