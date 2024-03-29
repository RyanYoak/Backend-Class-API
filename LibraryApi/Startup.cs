using AutoMapper;
using LibraryApi.AutomapperProfiles;
using LibraryApi.Controllers.Services;
using LibraryApi.Domain;
using LibraryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi
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

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Api", 
                    Version = "v1", 
                    Contact = new OpenApiContact{
                        Name = "Ryan Yoak",
                        Email = "ryan_yoak@progressive.com"
                    },
                    Description = "This is an API for my BES 100 course."
                    });
                //var xmlFile = $"{AssemblyLoadEventArgs.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = ConfigurationPath.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<ILookupServerStatus, HealthCheckServerStatus>();
            services.AddDbContext<LibraryDataContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("Library"));
            });

            var mapperConfig = new MapperConfiguration(options => {
                options.AddProfile(new BooksProfile());
            });

            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<MapperConfiguration>(mapperConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryApi v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
