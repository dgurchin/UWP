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
    public class DishMarkController : ControllerBase
    {
        private readonly DishMarkService _service;
        private readonly LogWebApiService _log;

        public DishMarkController(DishMarkService service, LogWebApiService log)
        {
            _service = service;
            _log = log;
        }

        // GET: api/DishMark
        [HttpGet("rowGuid")]
        public async Task<ActionResult<DishMark>> GetDishMarkAsync(Guid rowGuid)
        {
            DishMark rez = await _service.GetDishMarkAsync(rowGuid);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // GET: api/DishMark
        [HttpGet("dishGuid")]
        public async Task<ActionResult<IList<DishMark>>> GetDishMarksAsync(Guid dishGuid)
        {
            IList<DishMark> rez = await _service.GetDishMarksAsync(dishGuid);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // POST: api/DishMark
        [HttpPost]
        public async Task<ActionResult<DishMark>> Post(DishMark model)
        {
            DishMark insertDish = await _service.GetDishMarkAsync(model.RowGuid);
            if (insertDish != null)
            {
                _log.LogError("DishMark", "New", "Запись для блюда уже есть в списке.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            int rez = await _service.UpdateDishMarkAsync(model);
            if (rez > 0)
            {
                DishMark rezDishMark = await _service.GetDishMarkAsync(model.RowGuid);
                _log.LogInformation("DishMark", "New", "Запись добавлена.", $"зЗапись RowGuid={rezDishMark.RowGuid} добавлена.");
                return Ok(rezDishMark);
            }
            return BadRequest();
        }

        // PUT: api/DishMark
        [HttpPut]
        public async Task<ActionResult<DishMark>> Put(DishMark model)
        {
            DishMark updateDish = await _service.GetDishMarkAsync(model.RowGuid);
            if (updateDish == null)
            {
                _log.LogError("DishMark", "Update", "Запись для изменения не найдена.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            int rez = await _service.UpdateDishMarkAsync(model);
            if (rez > 0)
            {
                DishMark rezDishMark = await _service.GetDishMarkAsync(model.RowGuid);
                _log.LogInformation("DishMark", "Update", "Запись изменена", $"запись RowGuid={model.RowGuid} изменена.");
                return Ok(rezDishMark);
            }
            return BadRequest();
        }

        // DELETE: api/DishMark
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid DishMarkGuid)
        {
            DishMark delDishMark = await _service.GetDishMarkAsync(DishMarkGuid);
            if (delDishMark == null)
            {
                _log.LogError("DishMark", "Delete", "Запись для удаления не найдена.", $"RowGuid={DishMarkGuid}");
                return NotFound();
            }
            int rez = await _service.DeleteDishMarksAsync(delDishMark);
            _log.LogInformation("DishMark", "Delete", "Запись удалена", $"запись RowGuid={delDishMark.RowGuid} удалена.");
            return Ok();
        }
    }
}