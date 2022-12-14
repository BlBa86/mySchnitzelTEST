using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GCloud.Shared.Exceptions.Store
{
    public class ApiTokenInvalidException : BaseGustavException
    {
        public string ApiToken { get; set; }

        public ApiTokenInvalidException(string apiToken) : base(ExceptionStatusCode.ApiTokenInvalid, HttpStatusCode.NotFound, $"Für den ApiToken wurde keine Filiale gefunden.")
        {
            ApiToken = apiToken;
        }
    }
}
