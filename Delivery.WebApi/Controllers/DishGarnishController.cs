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
    [Route("api/[controller]")]
    [ApiController]
    public class DishGarnishController : ControllerBase
    {
        private readonly DishGarnishService _service;
        private readonly LogWebApiService _log;

        public DishGarnishController(DishGarnishService service, LogWebApiService log)
        {
            _service = service;
            _log = log;
        }

        // GET: api/Dish
        [HttpGet("DishGuid")]
        public async Task<ActionResult<IList<DishGarnish>>> GetDishGarnishesAsync(Guid dishGuid)
        {
            IList<DishGarnish> rez = await _service.GetDishGarnishesAsync(dishGuid);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // GET: api/Dish
        [HttpGet]
        public async Task<ActionResult<IList<DishGarnish>>> GetDishGarnishesAsync()
        {
            IList<DishGarnish> rez = await _service.GetDishGarnishesAsync();
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // POST: api/Dish
        [HttpPost("New")]
        public async Task<ActionResult<DishGarnish>> Post(DishGarnish model)
        {
            DishGarnish insertDish = await _service.GetDishGarnishAsync(model.RowGuid);
            if (insertDish != null)
            {
                _log.LogError("DishGarnish", "New", "Гарнир уже есть в списке гарниров.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            DishGarnish[] models = new DishGarnish[] { model };
            int rez = await _service.PostDishGarnishAsync(models);
            if (rez > 0)
            {
                DishGarnish rezDishGarnish = await _service.GetDishGarnishAsync(model.RowGuid);
                _log.LogInformation("DishGarnish", "New", "Гарнир добавлен.", $"Гарнир RowGuid={rezDishGarnish.RowGuid} '{rezDishGarnish.Name}' добавлен.");
                return Ok(rezDishGarnish);
            }
            return BadRequest();
        }

        // PUT: api/Dish/
        [HttpPut("Update")]
        public async Task<ActionResult<DishGarnish>> Put(DishGarnish model)
        {
            DishGarnish updateDish = await _service.GetDishGarnishAsync(model.RowGuid);
            if (updateDish == null)
            {
                _log.LogError("DishGarnish", "Update", "Гарнир для изменения не найден.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            DishGarnish[] models = new DishGarnish[] { model };
            int rez = await _service.PostDishGarnishAsync(models);
            if (rez > 0)
            {
                DishGarnish rezDishGarnish = await _service.GetDishGarnishAsync(model.RowGuid);
                _log.LogInformation("DishGarnish", "Update", "Гарнир изменен", $"Гарнир RowGuid={model.RowGuid} '{model.Name}' изменен.");
                return Ok(rezDishGarnish);
            }
            return BadRequest();
        }

        // DELETE: api/Dish/5
        [HttpDelete("Guid")]
        public async Task<ActionResult> Delete(Guid dishGarnishGuid)
        {
            DishGarnish delDishGarnish = await _service.GetDishGarnishAsync(dishGarnishGuid);
            if (delDishGarnish == null)
            {
                _log.LogError("DishGarnish", "Delete", "Гарнир для удаления не найден.", $"RowGuid={dishGarnishGuid}");
                return NotFound();
            }
            DishGarnish[] models = new DishGarnish[] { delDishGarnish };
            int rez = await _service.DeleteDishGarnishesAsync(models);
            _log.LogInformation("DishGarnish", "Delete", "Гарнир удален", $"Гарнир RowGuid={delDishGarnish.RowGuid} '{delDishGarnish.Name}' удален.");
            return Ok();
        }
    }
}
