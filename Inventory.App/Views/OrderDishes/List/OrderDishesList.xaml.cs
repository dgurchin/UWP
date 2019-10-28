﻿#region copyright
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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class OrderDishesList : UserControl
    {
        public OrderDishesList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderDishListViewModel ViewModel
        {
            get { return (OrderDishListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderDishListViewModel), typeof(OrderDishesList), new PropertyMetadata(null));
        #endregion

        #region DefaultCommands
        public string DefaultCommands
        {
            get { return (string)GetValue(DefaultCommandsProperty); }
            set { SetValue(DefaultCommandsProperty, value); }
        }

        public static readonly DependencyProperty DefaultCommandsProperty = DependencyProperty.Register(nameof(DefaultCommands), typeof(string), typeof(OrderDishesList), new PropertyMetadata("new,select,refresh,search"));
        #endregion
    }
}
