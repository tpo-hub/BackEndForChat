﻿using Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context) 
        {
            try
            {
              await next(context);
            }
            catch(Exception ex) 
            {
                await HandleExceptionAsync(context, ex, logger);
            }

        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;
            switch(ex)
            {
                case RestException re:
                    logger.LogError(ex, "REST ERROR");
                    errors = re.Error;
                    context.Response.StatusCode = (int)re.Code;
                    break;
                case Exception e:
                    logger.LogError(ex, "SERVER ERROR");
                    errors = string.IsNullOrEmpty(e.Message) ? "ERROR" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if(errors != null)
            {
                var result = JsonSerializer.Serialize(new { errors });
                await context.Response.WriteAsync(result);
            }


        }
    }
}
