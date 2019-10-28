
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
    public class DishIngredientController : ControllerBase
    {
        private readonly DishIngredientService _service;
        private readonly LogWebApiService _log;

        public DishIngredientController(DishIngredientService service, LogWebApiService log)
        {
            _service = service;
            _log = log;
        }

        // GET: api/DishIngredient
        [HttpGet("DishGuid")]
        public async Task<ActionResult<IList<DishIngredient>>> GetDishIngredientesAsync(Guid dishGuid)
        {
            IList<DishIngredient> rez = await _service.GetDishIngredientesAsync(dishGuid);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // GET: api/DishIngredient
        [HttpGet]
        public async Task<ActionResult<IList<DishIngredient>>> GetDishIngredientesAsync()
        {
            IList<DishIngredient> rez = await _service.GetDishIngredientesAsync();
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // POST: api/DishIngredient
        [HttpPost("New")]
        public async Task<ActionResult<DishIngredient>> Post(DishIngredient model)
        {
            DishIngredient insertDish = await _service.GetDishIngredientAsync(model.RowGuid);
            if (insertDish != null)
            {
                _log.LogError("DishIngredient", "New", "Инградиент уже есть в списке инградиентов.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            DishIngredient[] models = new DishIngredient[] { model };
            int rez = await _service.PostDishIngredientAsync(models);
            if (rez > 0)
            {
                DishIngredient rezDishIngredient = await _service.GetDishIngredientAsync(model.RowGuid);
                _log.LogInformation("DishIngredient", "New", "Инградиент добавлен.", $"Инградиент RowGuid={rezDishIngredient.RowGuid} '{rezDishIngredient.Name}' добавлен.");
                return Ok(rezDishIngredient);
            }
            return BadRequest();
        }

        // PUT: api/DishIngredient
        [HttpPut("Update")]
        public async Task<ActionResult<DishIngredient>> Put(DishIngredient model)
        {
            DishIngredient updateDish = await _service.GetDishIngredientAsync(model.RowGuid);
            if (updateDish == null)
            {
                _log.LogError("DishIngredient", "Update", "Инградиент для изменения не найден.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            DishIngredient[] models = new DishIngredient[] { model };
            int rez = await _service.PostDishIngredientAsync(models);
            if (rez > 0)
            {
                DishIngredient rezDishIngredient = await _service.GetDishIngredientAsync(model.RowGuid);
                _log.LogInformation("DishIngredient", "Update", "Инградиент изменен", $"Инградиент RowGuid={model.RowGuid} '{model.Name}' изменен.");
                return Ok(rezDishIngredient);
            }
            return BadRequest();
        }

        // DELETE: api/DishIngredient
        [HttpDelete("Guid")]
        public async Task<ActionResult> Delete(Guid DishIngredientGuid)
        {
            DishIngredient delDishIngredient = await _service.GetDishIngredientAsync(DishIngredientGuid);
            if (delDishIngredient == null)
            {
                _log.LogError("DishIngredient", "Delete", "Инградиент для удаления не найден.", $"RowGuid={DishIngredientGuid}");
                return NotFound();
            }
            DishIngredient[] models = new DishIngredient[] { delDishIngredient };
            int rez = await _service.DeleteDishIngredientsAsync(models);
            _log.LogInformation("DishIngredient", "Delete", "Инградиент удален", $"Инградиент RowGuid={delDishIngredient.RowGuid} '{delDishIngredient.Name}' удален.");
            return Ok();
        }
    }
}