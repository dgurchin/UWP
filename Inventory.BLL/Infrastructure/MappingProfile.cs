using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.WebApi.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>()
                .ForMember("Source", x => x.Ignore())
                .ForMember("Orders", x => x.Ignore());
        }
    }
}
