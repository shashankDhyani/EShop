using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ApiGateway.DTO
{
    public class AsyncRoutesOption
    {
        public bool Authenticate { get; set; }

        public Dictionary<string, AsyncRouteOption> Routes { get; set; }
    }

    public class AsyncRouteOption
    {
        public string Queue { get; set; }
    }
}
