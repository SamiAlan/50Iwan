using System;
using System.Collections.Generic;
using System.Net;

namespace Iwan.Client.Blazor.Infrastructure.Exceptions
{
    public class ServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public Dictionary<string, List<string>> Errors { get; set; }

        public ServiceException(string errorMessage, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, Dictionary<string, List<string>> errors = null)
        {
            ErrorMessage = errorMessage;
            StatusCode = httpStatusCode;
            Errors = errors ?? new Dictionary<string, List<string>>();
        }
    }
}
