using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Library.Api
{
    public interface IApiHelper
    { 
        HttpClient ApiClient { get;}
    }
}
