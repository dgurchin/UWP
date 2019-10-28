using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delivery.WebApi.Services;
using Inventory.BLL.DTO;
using Inventory.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly LogWebApiService _log;
        private readonly CustomerService _service;

        public CustomerController(CustomerService service, LogWebApiService log)
        {
            _log = log;
            _service = service;
        }

        #region Customer

        [HttpGet("{cutomerGuid}")]
        public async Task<CustomerDTO> GetCustomerDTOAsync(Guid cutomerGuid)
        {
            return await _service.GetCustomerDTOAsync(cutomerGuid); ;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDTO>>> GetCustomersRange()
        {
            List<CustomerDTO> rez = await _service.GetCustomersDTOAsync();
            if (rez == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rez);
            }
        }

        // POST: api/Customer/New
        [HttpPost()]
        public async Task<ActionResult<Guid>> Post(CustomerDTO model)
        {
            Customer insertCustomer = await _service.GetCustomerAsync(model.RowGuid);
            if (insertCustomer != null)
            {
                _log.LogError("Customer", "New", "Клиент для добавления уже есть в базе.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            IList<CustomerDTO> models = new CustomerDTO[] { model };
            int rez = await _service.PostCustomerDTOAsync(models);
            if (rez > 0)
            {
                Customer rezCustomer = await _service.GetCustomerAsync(model.RowGuid);
                _log.LogInformation("Customer", "New", "Клиент добавлен", $"Клиент RowGuid={rezCustomer.RowGuid} добавлен.");
                return Ok(rezCustomer.RowGuid);
            }
            return BadRequest();
        }

        // PUT: api/Customer/Update
        [HttpPut]
        public async Task<ActionResult<CustomerDTO>> Put(Guid customerGuid, CustomerDTO model)
        {
            Customer updateOrder = await _service.GetCustomerAsync(customerGuid);
            if (updateOrder == null)
            {
                _log.LogError("Customer", "Update", "Клиент для изменения не найден.", $"RowGuid='{customerGuid}'");
                return NotFound();
            }
            if (customerGuid != model.RowGuid)
            {
                _log.LogError("Customer", "Update", "Код клиента не соответстует данным для обновления.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            CustomerDTO[] models = new CustomerDTO[] { model };
            int rez = await _service.PostCustomerDTOAsync(models);
            if (rez > 0)
            {
                CustomerDTO rezCustomer = await _service.GetCustomerDTOAsync(customerGuid);
                _log.LogInformation("Customer", "Update", "Клиент изменен", $"Клиент RowGuid={model.RowGuid} изменен.");
                return Ok(rezCustomer);
            }
            return BadRequest();
        }
        #endregion Customer
    }
}
