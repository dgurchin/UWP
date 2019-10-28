using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Delivery.WebApi.Services;

using Inventory.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly MarkService _service;
        private readonly LogWebApiService _log;

        public MarkController(MarkService service, LogWebApiService log)
        {
            _service = service;
            _log = log;
        }

        // GET: api/Mark
        [HttpGet("Guid")]
        public async Task<ActionResult<Mark>> GetMarkAsync(Guid rowGuid)
        {
            Mark rez = await _service.GetMarkAsync(rowGuid);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // GET: api/Mark
        [HttpGet]
        public async Task<ActionResult<IList<Mark>>> GetMarksAsync()
        {
            IList<Mark> rez = await _service.GetMarksAsync();
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // POST: api/Mark
        [HttpPost("New")]
        public async Task<ActionResult<Mark>> Post(Mark model)
        {
            Mark insertDish = await _service.GetMarkAsync(model.RowGuid);
            if (insertDish != null)
            {
                _log.LogError("Mark", "New", "Особенность блюда уже есть в списке.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            int rez = await _service.UpdateMarkAsync(model);
            if (rez > 0)
            {
                Mark rezMark = await _service.GetMarkAsync(model.RowGuid);
                _log.LogInformation("Mark", "New", "Особенность добавлена.", $"особенность RowGuid={rezMark.RowGuid} добавлена.");
                return Ok(rezMark);
            }
            return BadRequest();
        }

        // PUT: api/Mark
        [HttpPut("Update")]
        public async Task<ActionResult<Mark>> Put(Mark model)
        {
            Mark updateDish = await _service.GetMarkAsync(model.RowGuid);
            if (updateDish == null)
            {
                _log.LogError("Mark", "Update", "Особенность для изменения не найдена.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            int rez = await _service.UpdateMarkAsync(model);
            if (rez > 0)
            {
                Mark rezMark = await _service.GetMarkAsync(model.RowGuid);
                _log.LogInformation("Mark", "Update", "Особенность изменена", $"особенность RowGuid={model.RowGuid} изменена.");
                return Ok(rezMark);
            }
            return BadRequest();
        }

        // DELETE: api/Mark
        [HttpDelete("Guid")]
        public async Task<ActionResult> Delete(Guid MarkGuid)
        {
            Mark delMark = await _service.GetMarkAsync(MarkGuid);
            if (delMark == null)
            {
                _log.LogError("Mark", "Delete", "Особенность для удаления не найдена.", $"RowGuid={MarkGuid}");
                return NotFound();
            }
            int rez = await _service.DeleteMarksAsync(delMark);
            _log.LogInformation("Mark", "Delete", "Особенность удалена", $"особенность RowGuid={delMark.RowGuid} удалена.");
            return Ok();
        }
    }
}