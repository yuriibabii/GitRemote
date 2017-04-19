using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitRemote.Services
{
    public static class ExtensionMethodsService
    {
        public static int GetIndexOfUrlParameter(this string[] parameters, string key)
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                if (parameter.Contains(key))
                    return i;
            }
            throw new ExceptionsService.SpecificParameterInUrlNotFoundException();
        }

        public static (string plainUrl, string parameters) GetPlainUrlAndParametersFromUrl(this string url)
        {
            if (!url.Contains("?"))
            {
                return new ValueTuple<string, string>(url, null);
            }

            var splited = url.Split('?');
            return new ValueTuple<string, string>(splited[0], splited[1]);
        }
    }
}
