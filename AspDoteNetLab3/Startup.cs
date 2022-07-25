using AspDotNetLab3.Extensions;
using AspDotNetLab3.Middleware;
using AspDotNetLab3.Worker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspDoteNetLab3
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; set; }

        public Startup(IConfiguration config)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("conf.json");
            AppConfiguration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), AppConfiguration["LogFile"]));
            var logger = loggerFactory.CreateLogger("FileLogger");

            app.UseSession();

            var routerBuilder = new RouteBuilder(app);

            routerBuilder.MapRoute("Session/Add/{key}/{value}", async context =>
            {
                await AddToSeesion(context);
            });

            routerBuilder.MapRoute("Session/View/{key}", async context =>
            {
                await GetSession(context);
            });

            routerBuilder.MapRoute("Cooke/Add/{key}/{value}", async context =>
            {
                await SetCooke(context);
            });

            routerBuilder.MapRoute("Cooke/View/{key}", async context =>
            {
                await GetCooke(context);
            });

            routerBuilder.MapRoute("{conntroller}/{action}/{id?}", async context =>
            {
                logger.LogInformation($" " +
                  $" {DateTime.Now}" +
                  $" {context.Request.Path}");
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("Запит {conntroller}/{action}/{?id}");
            });

            routerBuilder.MapRoute("{lang}/{conntroller}/{action}/{id?}", async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("Запит {lang}/{conntroller}/{action}/{?id}");
            });

            app.UseRouter(routerBuilder.Build());
        }

        private async Task AddToSeesion(HttpContext context)
        {
            try
            {
                var routeValues = context.GetRouteData().Values;
                var key = routeValues["key"].ToString();
                var value = routeValues["value"].ToString();
                context.Session.SetSession(key, value);
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("В сесію було додано дані");
            }
            catch (Exception erorr)
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync($"Erorr: {erorr}");
            }
        }

        private async Task GetSession(HttpContext context)
        {
            try
            {
                var routeValues = context.GetRouteData().Values;
                var key = routeValues["key"].ToString();
                var result = context.Session.GetSession(key);
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync($"Result: {result}");
            }
            catch (Exception erorr)
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync($"Erorr: {erorr}");
            }
        }

        private async Task SetCooke(HttpContext context)
        {
            try
            {
                var routeValues = context.GetRouteData().Values;
                var key = routeValues["key"].ToString();
                var value = routeValues["value"].ToString();
                context.Response.Cookies.Append(key, value);
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("В кукі було додано дані");
            }
            catch (Exception erorr)
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync($"Erorr: {erorr}");
            }
        }

        private async Task GetCooke(HttpContext context)
        {
            try
            {
                var routeValues = context.GetRouteData().Values;
                var key = routeValues["key"].ToString();
                string result = context.Request.Cookies[key];
                if (result == null)
                {
                    result = "Erorr key not found !";
                }
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync($"Result: {result}");
            }
            catch (Exception erorr)
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync($"Erorr: {erorr}");
            }
        }
    }
}
