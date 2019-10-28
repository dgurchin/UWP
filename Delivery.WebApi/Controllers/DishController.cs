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
    public class DishController : ControllerBase
    {
        private readonly DishService _service;
        private readonly LogWebApiService _log;

        public DishController(DishService service, LogWebApiService log)
        {
            _service = service;
            _log = log;
        }

        // GET: api/Dish/698a16e2-e878-11e6-80cb-6431504f2928
        [HttpGet("Guid")]
        public async Task<ActionResult<Dish>> GetDishAsync(Guid dishGuid)
        {
            Dish rez= await _service.GetDishAsync(dishGuid);
            if (rez==null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);

            }
        }

        // GET: api/Dish
        [HttpGet("quantity")]
        public async Task<ActionResult<IList<Dish>>> GetDishes(int quantity)
        {
            IList<Dish> rez= await _service.GetDishesAsync(0, quantity);
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
        [HttpGet("range")]
        public async Task<ActionResult<IList<Dish>>> GetDishesRange(int skip, int take)
        {
            IList<Dish> rez = await _service.GetDishesAsync(skip, take);
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
        public async Task<ActionResult<Dish>> Post(Dish model)
        {
            Dish insertDish = await _service.GetDishAsync(model.RowGuid);
            if (insertDish != null)
            {
                _log.LogError("Dish", "New", "Блюдо для добавления уже есть в базе.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            Dish[] models = new Dish[] { model };
            int rez = await _service.PostDishAsync(models);
            if (rez > 0)
            {
                Dish rezDish = await _service.GetDishAsync(model.RowGuid);
                _log.LogInformation("Dish", "New", "Блюдо добавлено", $"Блюдо RowGuid={rezDish.RowGuid} '{rezDish.Name}' добавлено.");
                return Ok(rezDish);
            }
            return BadRequest();
        }

        // PUT: api/Dish/
        [HttpPut("Update")]
        public async Task<ActionResult<Dish>> Put(Dish model)
        {
            Dish updateDish = await _service.GetDishAsync(model.RowGuid);
            if (updateDish == null)
            {
                _log.LogError("Dish", "Update", "Блюдо для изменения не найдено.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            Dish[] models = new Dish[] { model };
            int rez=await _service.PostDishAsync(models);
            if (rez > 0)
            {
                _log.LogInformation("Dish", "Update", "Блюдо изменено", $"Блюдо RowGuid={model.RowGuid} '{model.Name}' изменено.");
                Dish rezDish = await _service.GetDishAsync(model.RowGuid);
                return Ok(rezDish);
            }
            return BadRequest();
        }

        // DELETE: api/Dish/5
        [HttpDelete("Guid")]
        public async Task<ActionResult> Delete(Guid dishGuid)
        {
            Dish delDish = await _service.GetDishAsync(dishGuid);
            if (delDish == null)
            {
                _log.LogError("Dish", "Delete", "Блюдо для удаления не найдено.", $"RowGuid={dishGuid}");
                return NotFound();
            }
            Dish[] models = new Dish[] { delDish };
            int rez = await _service.DeleteDishesAsync(models);
            _log.LogInformation("Dish", "Delete", "Блюдо удалено", $"Блюдо RowGuid={delDish.RowGuid} '{delDish.Name}' удалено.");
            return Ok();
        }
    }
}
