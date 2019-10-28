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
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerCommunicationsController : ControllerBase
    {
        private readonly LogWebApiService _log;
        private readonly CustomerService _service;
        public CustomerCommunicationsController(CustomerService service, LogWebApiService log)
        {
            _log = log;
            _service = service;
        }
        #region Communication
        // GET: api/Customer/5
        [HttpGet("ID")]
        public async Task<Customer> GetCustomer(int id)
        {
            return await _service.GetCustomerAsync(id); ;
        }
        // GET: api/Communication/Guid?cutomerGuid=5db6b9a8-fe10-443f-b073-e97ca864f7b6
        [HttpGet("/api/Communication/Guid")]
        public async Task<Communication> GetCommunicationGuid(Guid communicationGuid)
        {
            return await _service.GetCommunicationAsync(communicationGuid); ;
        }
        // GET: api/Customer/Communication/Guid?cutomerGuid=5db6b9a8-fe10-443f-b073-e97ca864f7b6
        [HttpGet("Communication/Guid")]
        public async Task<IList<Communication>> GetCustomerCommunicationsGuid(Guid customerGuid)
        {
            return await _service.GetCustomerCommunicationsAsync(customerGuid); ;
        }
        // POST: api/Customer/Communication/New
        [HttpPost("Communication/New")]
        public async Task<ActionResult<Communication>> PostCommunication(Communication model)
        {
            Communication insertCommunication = await _service.GetCommunicationAsync(model.RowGuid);
            if (insertCommunication != null)
            {
                _log.LogError("Communication", "New", "Контакт для добавления уже есть в базе.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            IList<Communication> models = new Communication[] { model };
            int rez = await _service.PostCommunicationAsync(models);
            if (rez > 0)
            {
                Communication rezCommunication = await _service.GetCommunicationAsync(model.RowGuid);
                _log.LogInformation("Communication", "New", "Контакт добавлен", $"Контакт RowGuid={rezCommunication.RowGuid} добавлен.");
                return Ok(rezCommunication);
            }
            return BadRequest();
        }
        // PUT: api/Customer/Communication/Update
        [HttpPut("Communication/Update")]
        public async Task<ActionResult<Communication>> PutCommunication(Communication model)
        {
            Communication updateCommunication = await _service.GetCommunicationAsync(model.RowGuid);
            if (updateCommunication == null)
            {
                _log.LogError("Communication", "Update", "Контакт для изменения не найден.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            Communication[] models = new Communication[] { model };
            int rez = await _service.PostCommunicationAsync(models);
            if (rez > 0)
            {
                _log.LogInformation("Communication", "Update", "Контакт изменен", $"Контакт RowGuid={model.RowGuid} изменен.");
                Communication rezCommunication = await _service.GetCommunicationAsync(model.RowGuid);
                return Ok(rezCommunication);
            }
            return BadRequest();
        }
        // DELETE: api/Customer/Communication/Guid?cutomerGuid=5db6b9a8-fe10-443f-b073-e97ca864f7b6
        [HttpDelete("Communication/Guid")]
        public async Task<ActionResult> DeleteCommunication(Guid communicationGuid)
        {
            Communication delCommunication = await _service.GetCommunicationAsync(communicationGuid);
            if (delCommunication == null)
            {
                _log.LogError("Customer", "Delete", "Контакт для удаления не найден.", $"RowGuid={communicationGuid}");
                return NotFound();
            }
            Communication[] models = new Communication[] { delCommunication };
            await _service.DeleteCommunicationsAsync(models);
            _log.LogInformation("Communication", "Delete", "Контакт удален", $"Контакт RowGuid={delCommunication.RowGuid} удален.");
            return Ok();
        }
        #endregion Communication
        #region Address
        // GET: api/Communication/Guid?cutomerGuid=5db6b9a8-fe10-443f-b073-e97ca864f7b6
        [HttpGet("/api/Address/Guid")]
        public async Task<Address> GetAddressGuid(Guid addressGuid)
        {
            return await _service.GetAddressAsync(addressGuid); ;
        }

        // GET: api/Customer/Address/Guid?cutomerGuid=5db6b9a8-fe10-443f-b073-e97ca864f7b6
        [HttpGet("Address/Guid")]
        public async Task<IList<Address>> GetCustomerAddressesGuid(Guid customerGuid)
        {
            return await _service.GetCustomerAddressesAsync(customerGuid); ;
        }

        // POST: api/Customer/Address/New
        [HttpPost("Address/New")]
        public async Task<ActionResult<Address>> PostAddress(Address model)
        {
            Address insertAddress = await _service.GetAddressAsync(model.RowGuid);
            if (insertAddress != null)
            {
                _log.LogError("Address", "New", "Адрес для добавления уже есть в базе.", $"RowGuid='{model.RowGuid}'");
                return BadRequest();
            }
            IList<Address> models = new Address[] { model };
            int rez = await _service.PostAddressAsync(models);
            if (rez > 0)
            {
                Address rezAddress = await _service.GetAddressAsync(model.RowGuid);
                _log.LogInformation("Address", "New", "Адрес добавлен", $"Адрес RowGuid={rezAddress.RowGuid} добавлен.");
                return Ok(rezAddress);
            }
            return BadRequest();
        }
        // PUT: api/Customer/Address/Update
        [HttpPut("Address/Update")]
        public async Task<ActionResult<Address>> PutAddress(Address model)
        {
            Address updateAddress = await _service.GetAddressAsync(model.RowGuid);
            if (updateAddress == null)
            {
                _log.LogError("Address", "Update", "Адрес для изменения не найден.", $"RowGuid='{model.RowGuid}'");
                return NotFound();
            }
            Address[] models = new Address[] { model };
            int rez = await _service.PostAddressAsync(models);
            if (rez > 0)
            {
                _log.LogInformation("Address", "Update", "Адрес изменен", $"Контакт RowGuid={model.RowGuid} изменен.");
                Address rezAddress = await _service.GetAddressAsync(model.RowGuid);
                return Ok(rezAddress);
            }
            return BadRequest();
        }
        // DELETE: api/Customer/Address/?cutomerGuid=5db6b9a8-fe10-443f-b073-e97ca864f7b6
        [HttpDelete("Address/Guid")]
        public async Task<ActionResult> DeleteAddress(Guid addressGuid)
        {
            Address delAddress = await _service.GetAddressAsync(addressGuid);
            if (delAddress == null)
            {
                _log.LogError("Address", "Delete", "Адрес для удаления не найден.", $"RowGuid={addressGuid}");
                return NotFound();
            }
            //Address[] models = new Address[] { delAddress };
            await _service.DeleteAddressAsync(delAddress);
            _log.LogInformation("Address", "Delete", "Адрес удален", $"Адрес RowGuid={delAddress.RowGuid} удален.");
            return Ok();
        }
        #endregion Address
    }

}