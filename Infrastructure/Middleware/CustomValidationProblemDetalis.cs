using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    public class CustomValidationProblemDetalis : ProblemDetails
    {
        public IDictionary<string, string[]>? Errors { get; set; }

    }
}
