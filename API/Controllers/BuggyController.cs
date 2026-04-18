using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("un-authorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("Not a good request!");
        }

        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            return NotFound();
        }

        
        [HttpGet("internal-error")]
        public ActionResult GetInternalError()
        {
            throw new Exception("This is a test exceptions!");
        }

        [HttpPost("validation-error")]
        public ActionResult GetValidationError([FromBody] CreateProductDto product)
        {
            return Ok();
        }
    }
}
