#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public interface IOrderDishService
    {
        Task<OrderDishModel> GetOrderDishAsync(int orderDishId);
        Task<IList<OrderDishModel>> GetOrderDishesAsync(DataRequest<OrderDish> request);
        Task<IList<OrderDishModel>> GetOrderDishesAsync(int skip, int take, DataRequest<OrderDish> request);
        Task<int> GetOrderDishesCountAsync(DataRequest<OrderDish> request);

        Task<int> UpdateOrderDishAsync(OrderDishModel model);

        Task<int> DeleteOrderDishAsync(OrderDishModel model);
        Task<int> DeleteOrderDishRangeAsync(int index, int length, DataRequest<OrderDish> request);

        Task<decimal> GetOrderDishesSumAsync(Guid orderGuid);
    }
}
