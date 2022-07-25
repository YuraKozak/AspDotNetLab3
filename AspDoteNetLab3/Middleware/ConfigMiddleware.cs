using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetLab3.Middleware
{
    public class ConfigMiddleware
    {
        private readonly RequestDelegate _next;
        public IConfiguration AppConfiguration { get; set; }

        public ConfigMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            AppConfiguration = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsync(
                    $"Name: {AppConfiguration["name"]}\n" +
                    $"Age: {AppConfiguration["age"]}"
                );
        }
    }
}
