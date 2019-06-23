using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NHibernate;
using Restaurant.DataModel.DataFramework;
using Restaurant.DataModel.Entity;
using RestaurantApi.Support;
using Swashbuckle.AspNetCore.Swagger;

namespace RestaurantApi
{
    public class Startup
    {
        readonly string AllowCors = "AllowCors";

        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Dont suppress validation erorrs
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = false;
                
            });


            services.AddCors(options =>
            {
                options.AddPolicy(AllowCors,
                builder =>
                {
                    builder.WithOrigins("*")
                                          .AllowAnyHeader()
                                          .AllowAnyMethod(); 
                    
                });
               
            });
            
          

            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Restaurant API", Version = "v1" });
            });
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<AppSessionFactory>();
            services.AddScoped(x => x.GetService<AppSessionFactory>().SessionFactory);
            services.AddMvc().AddJsonOptions(options =>
              {
                  options.SerializerSettings.ContractResolver = new JsonPropertiesResolver();
                  options.SerializerSettings.Formatting = Formatting.Indented;
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           

            
            app.UseMiddleware(typeof(CorsMiddleware));


            app.UseHttpsRedirection();
            app.UseCors(AllowCors);
            app.UseStaticFiles();
            app.UseCookiePolicy();
       
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API V1");
            });
          
            app.UseMvc();

        }
    }
}
