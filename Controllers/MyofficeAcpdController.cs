using Microsoft.AspNetCore.Mvc;
using MyofficeApi.Models;
using MyofficeApi.Services;

namespace MyofficeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyofficeAcpdController : ControllerBase
    {
        private readonly IMyofficeAcpdService _service;

        public MyofficeAcpdController(IMyofficeAcpdService service)
        {
            _service = service;
        }

        // GET: api/myofficeacpd
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // GET: api/myofficeacpd/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST: api/myofficeacpd
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MyOfficeAcpd entity)
        {
            if (entity == null) return BadRequest("Invalid data");

            var newId = await _service.CreateAsync(entity);
            entity.ACPD_SID = newId;

            return CreatedAtAction(nameof(Get), new { id = newId }, entity);
        }

        // PUT: api/myofficeacpd/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] MyOfficeAcpd entity)
        {
            if (entity == null) return BadRequest();

            var updated = await _service.UpdateAsync(id, entity);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/myofficeacpd/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}