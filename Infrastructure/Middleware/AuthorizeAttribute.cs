using Application.Exceptions;
using Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;

        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = roles ?? new Role[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"]!;
            if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
            {
                var problem = new CustomValidationProblemDetalis
                {
                    Title = "Unauthorized",
                    Status = (int)HttpStatusCode.Unauthorized,
                    Detail = "please send Json Web Token from Header.",
                    Type = "Unauthorized"
                };

                context.Result = new JsonResult(problem)
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
