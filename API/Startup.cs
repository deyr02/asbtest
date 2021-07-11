using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CreditCards;
using FluentValidation.AspNetCore;
using MediatR;
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
using Persistence;

namespace API
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

            services.AddControllers().AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Create>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddDbContext<DataContext>(options =>
             {
                 var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                 string connStr;

                 // Depending on if in development or production, use either Heroku-provided
                 // connection string, or development connection string from env var.
                 if (env == "Development")
                 {
                     // Use connection string from file.
                     connStr = config.GetConnectionString("DefaultConnection");
                 }
                 else
                 {
                     // Use connection string provided at runtime by Heroku.
                     var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                     // Parse connection URL to connection string for Npgsql
                     connUrl = connUrl.Replace("postgres://", string.Empty);
                     var pgUserPass = connUrl.Split("@")[0];
                     var pgHostPortDb = connUrl.Split("@")[1];
                     var pgHostPort = pgHostPortDb.Split("/")[0];
                     var pgDb = pgHostPortDb.Split("/")[1];
                     var pgUser = pgUserPass.Split(":")[0];
                     var pgPass = pgUserPass.Split(":")[1];
                     var pgHost = pgHostPort.Split(":")[0];
                     var pgPort = pgHostPort.Split(":")[1];

                     connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";
                 }

                 // Whether the connection string came from the local development configuration file
                 // or from the environment variable from Heroku, use it to set up your DbContext.
                 options.UseNpgsql(connStr);
             });
            services.AddCors(opt =>
          {
              opt.AddPolicy("CorsPolicy", policy =>
              {
                  policy.AllowAnyMethod().AllowAnyHeader().WithOrigins(new string[] { "http://localhost:3000", "test" });
              });
          });

            services.AddMediatR(typeof(List.Handler).Assembly);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
