using ERP_Domain.Handle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERP_Api.Handler
{
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseErrorHandler(
                                          this IApplicationBuilder appBuilder,
                                          ILoggerFactory loggerFactory)
        {
            return appBuilder.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context
                                                    .Features
                                                    .Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature != null)
                    {
                        var logger = loggerFactory.CreateLogger("ErrorHandler");
                        logger.LogError($"Error: {exceptionHandlerFeature.Error}");

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var json = new
                        {
                            context.Response.StatusCode,
                            Message = exceptionHandlerFeature.Error is ServiceException ? exceptionHandlerFeature.Error.Message : "Internal Server Error",
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(json));
                    }
                });
            });
        }
    }
}
