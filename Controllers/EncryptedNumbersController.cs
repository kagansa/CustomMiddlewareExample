using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CustomMiddlewareExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptedNumbersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            if (HttpContext.Items["numberDecrypt"] != null)
            {
                return Ok((Convert.ToInt32(HttpContext.Items["numberDecrypt"]) * 2).ToString());
            }

            return BadRequest();
        }
    }
}