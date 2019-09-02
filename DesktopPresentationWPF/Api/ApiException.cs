using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.Api
{
    public class ApiErrorsException : Exception
    {
        private HttpResponseMessage _response;

        public ApiErrorsException(HttpResponseMessage response)
        {
            _response = response;
        }

        public HttpStatusCode StatusCode {
            get
            {
                return _response.StatusCode;
            }
        }
        public IEnumerable<string> Errors {
            get
            {
                return Data.Values.Cast<string>().ToList();
            }
        }
    }
}
