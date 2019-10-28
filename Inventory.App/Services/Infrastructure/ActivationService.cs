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

using Windows.ApplicationModel.Activation;

using Inventory.ViewModels;

namespace Inventory.Services
{
    #region ActivationInfo
    public class ActivationInfo
    {
        static public ActivationInfo CreateDefault() => Create<DashboardViewModel>();

        static public ActivationInfo Create<TViewModel>(object entryArgs = null) where TViewModel : ViewModelBase
        {
            return new ActivationInfo
            {
                EntryViewModel = typeof(TViewModel),
                EntryArgs = entryArgs
            };
        }

        public Type EntryViewModel { get; set; }
        public object EntryArgs { get; set; }
    }
    #endregion

    static public class ActivationService
    {
        static public ActivationInfo GetActivationInfo(IActivatedEventArgs args)
        {
            switch (args.Kind)
            {
                case ActivationKind.Protocol:
                    return GetProtocolActivationInfo(args as ProtocolActivatedEventArgs);

                case ActivationKind.Launch:
                default:
                    return ActivationInfo.CreateDefault();
            }
        }

        private static ActivationInfo GetProtocolActivationInfo(ProtocolActivatedEventArgs args)
        {
            if (args != null)
            {
                switch (args.Uri.AbsolutePath.ToLowerInvariant())
                {
                    case "customer":
                    case "customers":
                        int customerId = args.Uri.GetInt32Parameter("id");
                        if (customerId > 0)
                        {
                            return ActivationInfo.Create<CustomerDetailsViewModel>(new CustomerDetailsArgs { CustomerId = customerId });
                        }
                        return ActivationInfo.Create<CustomersViewModel>(new CustomerListArgs());
                    case "order":
                        string orderKey = args.Uri.GetParameter("id");
                        if (Guid.TryParse(orderKey, out Guid orderGuid))
                        {
                            return ActivationInfo.Create<OrdersViewModel>(new OrderDetailsArgs { OrderGuid = orderGuid });
                        }
                        // TODO: Проверить условие, что сдесь не OrderId:int
                        return ActivationInfo.Create<OrdersViewModel>(new OrderListArgs());
                    case "orders":
                        return ActivationInfo.Create<OrdersViewModel>(new OrderListArgs());
                    case "product":
                    case "dish":
                    case "products":
                    case "dishes":
                        int productId = args.Uri.GetInt32Parameter("id");
                        if (productId > 0)
                        {
                            return ActivationInfo.Create<DishDetailsViewModel>(new DishDetailsArgs { DishId = productId });
                        }
                        return ActivationInfo.Create<DishesViewModel>(new DishListArgs());
                }
            }
            return ActivationInfo.CreateDefault();
        }
    }
}
