using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamsController : ControllerBase
    {
        private readonly Streams _streams;

        public StreamsController(Streams streams)
        {
            _streams = streams;
        }

        [HttpGet("{name}/{id}")]
        public async Task<IActionResult> Get(string name, int id)
        {
            try
            {
                var result = await _streams.Read(name, id);

                return Ok(result);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{name}")]
        public async Task<IActionResult> Post(string name, [FromBody] Event @event)
        {
            try
            {
                var result = await _streams.WriteTo(name, @event);

                return Created($"/streams/{name}/{result.NextExpectedVersion}", null);
            }
            catch (WrongExpectedVersionException)
            {
                return BadRequest();
            }
        }
    }
}
