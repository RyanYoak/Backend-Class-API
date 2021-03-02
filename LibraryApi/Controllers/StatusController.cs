using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class StatusController : ControllerBase
    {
        private readonly ILookupServerStatus _statusLookup;

        public StatusController(ILookupServerStatus statusLookup)
        {
            _statusLookup = statusLookup;
        }

        // GET /status
        [HttpGet("status")]
        public StatusResponse GetTheStatus()
        {
            var status = _statusLookup.GetStatus();
            return (StatusResponse)status;
        }


        [HttpGet("cutomers/{customerId:int}")]
        public ActionResult GetInfoAboutCustomer(int customerId)
        {
            return Ok($"Getting info about customer {customerId}");
        }

        [HttpGet("blogs/{year:int}/{month:int}/{day:int}")]
        public ActionResult GetBlogPost(int year, int month, int day)
        {
            return Ok($"Getting info about blogs at {day}/{month}/{year}");
        }

        [HttpGet("employees")]
        public ActionResult GetEmployees([FromQuery] string department = "All")
        {
            var response = new GetEmployeesResponse
            {
                Data = new List<string> { "joe", "mary", "sue" },
                Department = department
            };
            return Ok(response);
        }

        [HttpGet("whoami")]
        public ActionResult WhoAmI([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok($"I am {userAgent}");
        }

        [HttpPost("employee")]
        public ActionResult Hire([FromBody] Employee request)
        {
            return Ok($"Hiring {request.Name} in {request.Department} for {request.StartingSalary:c}.");
        }
    }

    public class StatusResponse
    {
        public string Message { get; set; }
        public DateTime LastChecked { get; set; }
    }

    public class GetEmployeesResponse
    {
        public List<string> Data { get; set; }
        public string Department { get; set; }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal StartingSalary { get; set; }
    }
}