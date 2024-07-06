using System.Net;

namespace Iwan.Server.Infrastructure
{
    public class ServiceResponse<TData> where TData : class
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }

        public ServiceResponse(TData data = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            Data = data;
            StatusCode = httpStatusCode;
        }

        public ServiceResponse(string message, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }

    public class NotFoundResponse<TData> : ServiceResponse<TData> where TData : class
    {
        public NotFoundResponse(string message) : base(message, HttpStatusCode.NotFound) { }
    }

    public class BadRequestResponse<TData> : ServiceResponse<TData> where TData : class
    {
        public BadRequestResponse(string message) : base(message, HttpStatusCode.BadRequest) { }
    }

    public class ConflictResponse<TData> : ServiceResponse<TData> where TData : class
    {
        public ConflictResponse(string message) : base(message, HttpStatusCode.Conflict) { }
    }

    public class ServerErrorResponse<TData> : ServiceResponse<TData> where TData : class
    {
        public ServerErrorResponse(string message) : base(message, HttpStatusCode.InternalServerError) { }
    }
}
