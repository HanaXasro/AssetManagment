using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NotFoundEx : Exception
    {
        public NotFoundEx(string name, object key) : base($"{name} ({key})") { }
    }
}
