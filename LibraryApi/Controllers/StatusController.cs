using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class StatusController : ControllerBase
    {
        // GET /status
        [HttpGet("status")]
        public string Getstatus()
        {
            return "Looks good!";
        }
    }
}
