using LibraryApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers.Services
{
    public class HealthCheckServerStatus : ILookupServerStatus
    {
        public StatusResponse GetStatus()
        {
            return new StatusResponse
            {
                Message = "Everything is going great. Thanks for asking!",
                LastChecked = DateTime.Now
            };
        }
    }
}
