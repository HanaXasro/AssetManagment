using Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute(params string[] permissions) : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = (User?)context.HttpContext.Items["User"];
        if (user == null)
            SetUnauthorizedResult(context, "User not found in context. Please provide a valid JWT.");

        var dbContext = context.HttpContext.RequestServices.GetRequiredService<DataDbContext>();
        var hasPermission = await dbContext.RolePermissions
            .AnyAsync(e => e.RoleId == user!.RoleId && permissions.Contains(e.Permission!.Title));

        if (!hasPermission)
            SetForbiddenResult(context, "You do not have the required permissions.");
    }

    private void SetUnauthorizedResult(AuthorizationFilterContext context, string detail)
    {
        var problem = new CustomProblemDetails
        {
            Title = "Unauthorized",
            Status = (int)HttpStatusCode.Unauthorized,
            Detail = detail,
            Type = "Unauthorized"
        };

        context.Result = new JsonResult(problem)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };
    }
    
    private void SetForbiddenResult(AuthorizationFilterContext context, string detail)
    {
        var problem = new CustomProblemDetails
        {
            Title = "Forbidden",
            Status = (int)HttpStatusCode.Forbidden,
            Detail = detail,
            Type = "Forbidden"
        };

        context.Result = new JsonResult(problem)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
}