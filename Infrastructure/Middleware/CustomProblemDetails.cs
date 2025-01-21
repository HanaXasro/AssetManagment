using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    public class CustomProblemDetails : ProblemDetails
    {
        public IDictionary<string, string[]>? Errors { get; set; }

    }
}
