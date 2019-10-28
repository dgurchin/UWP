using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Delivery.WebApi.Services;

using Inventory.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;
        private readonly LogWebApiService _log;

        public OrderController(OrderService service, LogWebApiService log)
        {
            _service = service;
            _log = log;
        }

        #region Order
        // GET: api/Order/orderId?4
        [HttpGet("orderId")]
        public async Task<ActionResult<Order>> GetOrderIdAsync(int orderId)
        {
            Order rez = await _service.GetOrderAsync(orderId);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);

            }
        }

        // GET: api/Order/Guid?698a16e2-e878-11e6-80cb-6431504f2928
        [HttpGet("Guid")]
        public async Task<ActionResult<Order>> GetOrderGuidAsync(Guid orderGuid)
        {
            Order rez = await _service.GetOrderAsync(orderGuid);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);

            }
        }

        // GET: api/Order/today
        [HttpGet("today")]
        public async Task<ActionResult<IList<Order>>> GetOrdersToday()
        {
            IList<Order> rez = await _service.GetOrdersTodayAsync();
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // GET: api/Order/today/restaurant
        [HttpGet("today/restaurant")]
        public async Task<ActionResult<IList<Order>>> GetRestauranOrdersToday(int restaurantId)
        {
            IList<Order> rez = await _service.GetRestauantOrdersTodayAsync(restaurantId);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // GET: api/Order/today/restaurant/status
        [HttpGet("today/restaurant/status")]
        public async Task<ActionResult<IList<Order>>> GetRestauranOrdersStatusToday(int restaurantID,int statusId)
        {
            IList<Order> rez = await _service.GetRestauantOrdersTodayAsync(restaurantID, statusId);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // GET: api/Order/
        [HttpGet("quantity")]
        public async Task<ActionResult<IList<Order>>> GetOrders(int quantity)
        {
            IList<Order> rez = await _service.GetOrdersAsync(0, quantity);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // GET: api/Order
        [HttpGet("range")]
        public async Task<ActionResult<IList<Order>>> GetOrdersRange(int skip, int take)
        {
            IList<Order> rez = await _service.GetOrdersAsync(skip, take);
            if (rez == null)
            {
                await _log.WriteAsync(LogType.Warning, nameof(OrderController), nameof(GetOrdersRange), "Orders is null", "Items count");
                return NotFound();
            }
            else
            {
                await _log.WriteAsync(LogType.Information, nameof(OrderController), nameof(GetOrdersRange), rez.Count.ToString(), "Items count");
                return Ok(rez);
            }
        }

        // GET: api/Order/
        [HttpGet("CustomerOrders")]
        public async Task<ActionResult<IList<Order>>> GetCustomerOrders(Guid customerGuid,int skip, int take)
        {

            IList<Order> rez = await _service.GetCustomerOrdersAsync(customerGuid, skip, take);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // POST: api/Order
        [HttpPost("New")]
        public async Task<ActionResult<Order>> Post(Order model)
        {
            Order insertOrder = await _service.GetOrderAsync(model.RowGuid);
            if (insertOrder != null)
            {
                _log.LogError("Order", "New", "Заказ для добавления уже есть в базе.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            Order[] models = new Order[] { model };
            int rez = await _service.PostOrderAsync(models);
            if (rez > 0)
            {
                Order rezOrder = await _service.GetOrderAsync(model.RowGuid);
                _log.LogInformation("Order", "New", "Заказ добавлен", $"Заказ RowGuid={rezOrder.RowGuid} добавлен.");
                return Ok(rezOrder);
            }
            return BadRequest();
        }



        // PUT: api/Order/
        [HttpPut("Update")]
        public async Task<ActionResult<Order>> Put(Order model)
        {
            Order updateOrder = await _service.GetOrderAsync(model.RowGuid);
            if (updateOrder == null)
            {
                _log.LogError("Order", "Update", "Заказ для изменения не найден.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            Order[] models = new Order[] { model };
            int rez = await _service.PostOrderAsync(models);
            if (rez > 0)
            {
                _log.LogInformation("Order", "Update", "Заказ изменен", $"Заказ RowGuid={model.RowGuid} изменен.");
                Order rezOrder = await _service.GetOrderAsync(model.RowGuid);
                return Ok(rezOrder);
            }
            return BadRequest();
        }

        // DELETE: api/Order/5
        [HttpDelete("Guid")]
        public async Task<ActionResult> Delete(Guid orderGuid)
        {
            Order delOrder = await _service.GetOrderAsync(orderGuid);
            if (delOrder == null)
            {
                _log.LogError("Order", "Delete", "Заказ для удаления не найден.", $"RowGuid={orderGuid}");
                return NotFound();
            }
            Order[] models = new Order[] { delOrder };
            int rez = await _service.DeleteOrdersAsync(models);
                _log.LogInformation("Order", "Delete", "Заказ удален", $"Заказ RowGuid={delOrder.RowGuid} удален.");
            return Ok();
        }
        #endregion Order
        #region OrderDish
        // GET: api/Order/OrderDishes/698a16e2-e878-11e6-80cb-6431504f2928
        [HttpGet("OrderDishes")]
        public async Task<ActionResult<OrderDish>> GetOrderDishesAsync(Guid orderGuid)
        {
            IList<OrderDish> rez = await _service.GetOrderDishesAsync(orderGuid);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);

            }
        }

        // GET: api/Order/OrderDishGarnishes/698a16e2-e878-11e6-80cb-6431504f2928
        [HttpGet("OrderDishGarnishes")]
        public async Task<ActionResult<OrderDishGarnish>> GetOrderDishGarnishesAsync(int orderDishID)
        {
            IList<OrderDishGarnish> rez = await _service.GetOrderDishGarnishesAsync(orderDishID);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);

            }
        }

        // GET: api/Order/OrderDishGarnishes/698a16e2-e878-11e6-80cb-6431504f2928
        [HttpGet("OrderDishIngredients")]
        public async Task<ActionResult<OrderDishIngredient>> GetOrderDishIngredientsAsync(int orderDishID)
        {
            IList<OrderDishIngredient> rez = await _service.GetOrderDishIngredientsAsync(orderDishID);
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);

            }
        }
        #endregion OrderDish
    }
}
