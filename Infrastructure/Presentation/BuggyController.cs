using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Presentation
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")] //GET : api/ Buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            //Code
            return NotFound(); //400

        }

        [HttpGet("severerror")] //GET : api/ Buggy/severerror
        public IActionResult GetServerErrorRequest()
        {

            throw new Exception();
            return Ok();


        }

        [HttpGet(template: "badrequest")] //GET : api/ Buggy/badrequest
        public IActionResult GetBadRequest()
        {
            //Code
            return BadRequest(); //404

        }

        [HttpGet(template: "badrequest/{id}")] //GET : api/ Buggy/badrequest/2 
        public IActionResult GetBadRequest(int id) // Validtion Error
        {
            //Code
            return BadRequest(); //404

        }

        [HttpGet(template: "unauthorized")] //GET : api/ Buggy/unauthorized 
        public IActionResult GetUnauthorizedRequest(int id) // Validtion Error
        {
            //Code
            return Unauthorized(); //401

        }
    }
}
