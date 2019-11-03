using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library
{
    internal static class HttpRequestMessageExtensions
    {
        public static void SetAuthHeaders(this HttpRequestMessage r, string token)
        {
            r.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
        }
    }
}
