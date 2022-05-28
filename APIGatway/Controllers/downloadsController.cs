using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGatway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class downloadsController : ControllerBase
    {
        [HttpGet]
        public String Get(string MyQueryParameter)
        {
            var allH = Request.Headers;
            return $"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA--{DateTime.Now.ToLongTimeString()}";
        }
    }
}
