using Iwan.Server.Services.Accounts;
using Iwan.Shared;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Iwan.Server.Infrastructure.Middlewares
{
    /// <summary>
    /// Represents a global exception handling middleware
    /// </summary>
    public class ExceptionsHandlingMiddleware : IMiddleware
    {
        protected readonly IStringLocalizer<Localization> _stringLocalizer;
      public ExceptionsHandlingMiddleware(IStringLocalizer<Localization> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        /// <summary>
        /// Invoked when the middleware is called
        /// </summary>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Go to the next middelware
                await next(context);
            }
            catch (Exception ex)
            {
                // Handle the exception whatever it was
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the passed exception
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Initial status code
            var statusCode = (int)HttpStatusCode.InternalServerError;

            // Initial message
            var message = "Server error";

            // Properties errors
            var errors = new Dictionary<string, List<string>>();

            // If the exception was a base application exception
            if (ex is BaseException baseException)
            {
                // Get the status code
                statusCode = baseException.StatusCode;

                // Get the message
                message = baseException.Values?.Any() ?? false
                    ? _stringLocalizer.Localize(baseException.Message, baseException.Values)
                    : _stringLocalizer.Localize(baseException.Message);

                if (!string.IsNullOrEmpty(baseException.PropertyName))
                    errors.Add(baseException.PropertyName, new List<string> { message });
            }

            // Create an api error response and jsonize it
            var result = new ApiErrorResponse(message, errors: errors).ToJson();

            // Set the content type
            context.Response.ContentType = "application/json";

            // Set the status code
            context.Response.StatusCode = statusCode;

            // Write the error response
            await context.Response.WriteAsync(result);
        }
    }
}
