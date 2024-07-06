using Iwan.Shared.Constants;
using System;
using System.Net;

namespace Iwan.Shared.Exceptions
{
    /// <summary>
    /// Represents the base application exception
    /// </summary>
    public class BaseException : Exception
    {
        /// <summary>
        /// The <see cref="HttpStatusCode"/> as an integer
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// The values passed to be fit to the final message
        /// </summary>
        public object[] Values { get; }

        public string PropertyName { get; }

        /// <summary>
        /// Constructs a new instance of the <see cref="BaseException"/> class using the passed parameters
        /// </summary>
        public BaseException(string message, HttpStatusCode statusCode) : base(message)
        {
            // Set the status code
            StatusCode = (int)statusCode;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BaseException"/> class using the passed parameters
        /// </summary>
        public BaseException(string message, HttpStatusCode statusCode, params object[] values) : base(message)
        {
            // Set the status code
            StatusCode = (int)statusCode;

            // Set the values
            Values = values;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BaseException"/> class using the passed parameters
        /// </summary>
        public BaseException(string message, HttpStatusCode statusCode, string propertyName = null, params object[] values) : base(message)
        {
            // Set the status code
            StatusCode = (int)statusCode;

            // Set the values
            Values = values;

            // Set the property name
            PropertyName = propertyName;
        }
    }

    /// <summary>
    /// Represents a not found exception
    /// </summary>
    public class NotFoundException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NotFoundException"/> class using the passed parameters
        /// </summary>
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NotFoundException"/> class using the passed parameters
        /// </summary>
        public NotFoundException(string message, params object[] values) : base(message, HttpStatusCode.NotFound, values) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NotFoundException"/> class using the passed parameters
        /// </summary>
        public NotFoundException(string message, string propertyName, params object[] values) : base(message, HttpStatusCode.NotFound, propertyName, values) { }
    }

    /// <summary>
    /// Represents an already exists exception
    /// </summary>
    public class AlreadyExistException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="AlreadyExistException"/> class using the passed parameters
        /// </summary>
        public AlreadyExistException(string message) : base(message, HttpStatusCode.Conflict) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="AlreadyExistException"/> class using the passed parameters
        /// </summary>
        public AlreadyExistException(string message, params object[] values) : base(message, HttpStatusCode.Conflict, values) { }
        
        /// <summary>
        /// Constructs a new instance of the <see cref="AlreadyExistException"/> class using the passed parameters
        /// </summary>
        public AlreadyExistException(string message, string propertyName, params object[] values) : base(message, HttpStatusCode.Conflict, propertyName, values) { }
    }

    /// <summary>
    /// Represents an unauthorized exception
    /// </summary>
    public class UnAuthorizedException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="UnAuthorizedException"/> class using the passed parameters
        /// </summary>
        public UnAuthorizedException(string message = ValidationResponses.General.UnAuthorizedAction) 
            : base(message, HttpStatusCode.Unauthorized) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NotFoundException"/> class using the passed parameters
        /// </summary>
        public UnAuthorizedException(string message, string propertyName, params object[] values) : base(message, HttpStatusCode.Conflict, propertyName, values) { }
    }

    /// <summary>
    /// Represents a bad request exception
    /// </summary>
    public class BadRequestException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="BadRequestException"/> class using the passed parameters
        /// </summary>
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="BadRequestException"/> class using the passed parameters
        /// </summary>
        public BadRequestException(string message, params object[] values) : base(message, HttpStatusCode.BadRequest, values) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="BadRequestException"/> class using the passed parameters
        /// </summary>
        public BadRequestException(string message, string propertyName, params object[] values) : base(message, HttpStatusCode.BadRequest, propertyName, values) { }
    }

    /// <summary>
    /// Represents a forbiddenexception
    /// </summary>
    public class ForbiddenException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ForbiddenException"/> class using the passed parameters
        /// </summary>
        public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NotFoundException"/> class using the passed parameters
        /// </summary>
        public ForbiddenException(string message, params object[] values) : base(message, HttpStatusCode.Forbidden, values) { }
    }

    /// <summary>
    /// Represents a conflict exception
    /// </summary>
    public class ConflictException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ConflictException"/> class using the passed parameters
        /// </summary>
        public ConflictException(string message) : base(message, HttpStatusCode.Conflict) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ConflictException"/> class using the passed parameters
        /// </summary>
        public ConflictException(string message, params object[] values) : base(message, HttpStatusCode.Conflict, values) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ConflictException"/> class using the passed parameters
        /// </summary>
        public ConflictException(string message, string propertyName, params object[] values) : base(message, HttpStatusCode.Conflict, propertyName, values) { }
    }

    /// <summary>
    /// Represents a sevice exception
    /// </summary>
    public class ServiceUnavailableException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ServiceUnavailableException"/> class using the passed parameters
        /// </summary>
        public ServiceUnavailableException(string message) : base(message, HttpStatusCode.ServiceUnavailable) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NotFoundException"/> class using the passed parameters
        /// </summary>
        public ServiceUnavailableException(string message, params object[] values) : base(message, HttpStatusCode.ServiceUnavailable, values) { }
    }

    /// <summary>
    /// Represents a sevice exception
    /// </summary>
    public class ServerErrorException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ServerErrorException"/> class using the passed parameters
        /// </summary>
        public ServerErrorException(string message) : base(message, HttpStatusCode.InternalServerError) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="NotFoundException"/> class using the passed parameters
        /// </summary>
        public ServerErrorException(string message, params object[] values) : base(message, HttpStatusCode.InternalServerError, values) { }
    }
}
