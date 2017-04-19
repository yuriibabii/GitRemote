using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GitRemote.Services.StringService.SoftStrings;

namespace GitRemote.Services
{
    public static class ExceptionsService
    {
        public class ActionTypeNotFoundException : Exception
        {
            public ActionTypeNotFoundException()
            {

            }
            public ActionTypeNotFoundException(string message) : base(message)
            {

            }

            public ActionTypeNotFoundException(string message, Exception inner) : base(message, inner)
            {

            }
        }

        public class SpecificParameterInUrlNotFoundException : Exception
        {
            public SpecificParameterInUrlNotFoundException()
            {

            }
            public SpecificParameterInUrlNotFoundException(string message) : base(message)
            {

            }

            public SpecificParameterInUrlNotFoundException(string message, Exception inner) : base(message, inner)
            {

            }
        }
    }
}
