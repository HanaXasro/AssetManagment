using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    public class GlobalException(RequestDelegate requestDelegate)
    {

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await requestDelegate(httpContext);
            }
            catch (Exception ex)
            {

                HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
                CustomProblemDetails problem;
                switch (ex)
                {
                    case BadRequestException badRequest:
                        httpStatusCode = HttpStatusCode.BadRequest;
                        problem = new CustomProblemDetails
                        {
                            Title = badRequest.Message,
                            Status = (int)httpStatusCode,
                            Detail = badRequest.InnerException?.Message,
                            Type = nameof(BadRequest),
                            Errors = badRequest.Errors
                        };
                        break;

                    case NotFoundException notFound:
                        httpStatusCode = HttpStatusCode.NotFound;
                        problem = new CustomProblemDetails
                        {
                            Title = notFound.Message,
                            Status = (int)httpStatusCode,
                            Detail = notFound.InnerException?.Message,
                            Type = nameof(NotFound),
                        };

                        break;
                    case UnprocessableException unprocessable:
                        httpStatusCode = HttpStatusCode.UnprocessableEntity;
                        problem = new CustomProblemDetails
                        {
                            Title = unprocessable.Message,
                            Status = (int)httpStatusCode,
                            Detail = unprocessable.InnerException?.Message,
                            Type = nameof(UnprocessableEntity),
                        };

                        break;

                    case UnauthorizedException unauthorizedEx:
                        httpStatusCode = HttpStatusCode.Unauthorized;
                        problem = new CustomProblemDetails
                        {
                            Title = unauthorizedEx.Message,
                            Status = (int)httpStatusCode,
                            Detail = unauthorizedEx.InnerException?.Message,
                            Type = "Unauthorized"
                        };

                        break;

                    default:

                        problem = new CustomProblemDetails
                        {
                            Title = ex.Message,
                            Status = (int)httpStatusCode,
                            Type = "Unhandled Exception",
                            Detail = ex.StackTrace
                        };
                        break;
                }
                httpContext.Response.StatusCode = (int)httpStatusCode;
                await httpContext.Response.WriteAsJsonAsync(problem);
            }
        
        }
    }
}
