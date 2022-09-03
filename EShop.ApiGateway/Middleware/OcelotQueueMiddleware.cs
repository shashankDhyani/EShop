using EShop.ApiGateway.DTO;
using EShop.Infrastructure.Command.Product;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EShop.ApiGateway.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class OcelotQueueMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, AsyncRouteOption> _asyncRoutes;
        public OcelotQueueMiddleware(RequestDelegate next, IOptions<AsyncRoutesOption> asyncRoutesOptions)
        {
            _next = next;
            _asyncRoutes = asyncRoutesOptions.Value.Routes;
        }

        public async Task Invoke(HttpContext httpContext, IPublishEndpoint publishEndpoint)
        {
            try
            {
                if (httpContext.Request.Method == HttpMethod.Post.ToString())
                {
                    var reader = new StreamReader(httpContext.Request.Body);
                    var content = await reader.ReadToEndAsync();

                    var payload = JsonConvert.DeserializeObject<CreateProduct>(content);

                    await publishEndpoint.Publish(payload);
                    httpContext.Response.StatusCode = 201;
                    await httpContext.Response.WriteAsync("Your request is accepted");
                }
                else
                {
                    await _next(httpContext);
                }
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = 501;
                await httpContext.Response.WriteAsync($"Error: {ex.Message}");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class OcelotQueueMiddlewareExtensions
    {
        public static IApplicationBuilder UseOcelotQueueMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OcelotQueueMiddleware>();
        }
    }
}
