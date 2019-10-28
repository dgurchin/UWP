﻿#pragma checksum "D:\Delivery\src\Inventory.App\Views\Order\OrderView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "91901D092D3BC13215F69F57DB8A8F21"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory.Views
{
    partial class OrderView : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Inventory_Views_OrderDetails_ViewModel(global::Inventory.Views.OrderDetails obj, global::Inventory.ViewModels.OrderDetailsWithItemsViewModel value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Inventory.ViewModels.OrderDetailsWithItemsViewModel) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Inventory.ViewModels.OrderDetailsWithItemsViewModel), targetNullValue);
                }
                obj.ViewModel = value;
            }
            public static void Set_Inventory_Controls_WindowTitle_Title(global::Inventory.Controls.WindowTitle obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Title = value ?? global::System.String.Empty;
            }
            public static void Set_Windows_UI_Xaml_Controls_Control_IsEnabled(global::Windows.UI.Xaml.Controls.Control obj, global::System.Boolean value)
            {
                obj.IsEnabled = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class OrderView_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IDataTemplateComponent,
            global::Windows.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IOrderView_Bindings
        {
            private global::Inventory.Views.OrderView dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Inventory.Controls.WindowTitle obj2;
            private global::Inventory.Controls.Section obj3;
            private global::Inventory.Views.OrderDetails obj4;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj2TitleDisabled = false;
            private static bool isobj3IsEnabledDisabled = false;
            private static bool isobj4ViewModelDisabled = false;

            private OrderView_obj1_BindingsTracking bindingsTracking;

            public OrderView_obj1_Bindings()
            {
                this.bindingsTracking = new OrderView_obj1_BindingsTracking(this);
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 16 && columnNumber == 31)
                {
                    isobj2TitleDisabled = true;
                }
                else if (lineNumber == 18 && columnNumber == 27)
                {
                    isobj3IsEnabledDisabled = true;
                }
                else if (lineNumber == 20 && columnNumber == 50)
                {
                    isobj4ViewModelDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 2: // Views\Order\OrderView.xaml line 16
                        this.obj2 = (global::Inventory.Controls.WindowTitle)target;
                        break;
                    case 3: // Views\Order\OrderView.xaml line 18
                        this.obj3 = (global::Inventory.Controls.Section)target;
                        break;
                    case 4: // Views\Order\OrderView.xaml line 20
                        this.obj4 = (global::Inventory.Views.OrderDetails)target;
                        break;
                    default:
                        break;
                }
            }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                throw new global::System.NotImplementedException();
            }

            public void Recycle()
            {
                throw new global::System.NotImplementedException();
            }

            // IOrderView_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                if (newDataRoot != null)
                {
                    this.dataRoot = (global::Inventory.Views.OrderView)newDataRoot;
                    return true;
                }
                return false;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::Inventory.Views.OrderView obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel(obj.ViewModel, phase);
                    }
                }
            }
            private void Update_ViewModel(global::Inventory.ViewModels.OrderDetailsWithItemsViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ViewModel(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_OrderDetails(obj.OrderDetails, phase);
                    }
                }
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Order\OrderView.xaml line 20
                    if (!isobj4ViewModelDisabled)
                    {
                        XamlBindingSetters.Set_Inventory_Views_OrderDetails_ViewModel(this.obj4, obj, null);
                    }
                }
            }
            private void Update_ViewModel_OrderDetails(global::Inventory.ViewModels.OrderDetailsViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ViewModel_OrderDetails(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_OrderDetails_Title(obj.Title, phase);
                        this.Update_ViewModel_OrderDetails_IsEnabled(obj.IsEnabled, phase);
                    }
                }
            }
            private void Update_ViewModel_OrderDetails_Title(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Order\OrderView.xaml line 16
                    if (!isobj2TitleDisabled)
                    {
                        XamlBindingSetters.Set_Inventory_Controls_WindowTitle_Title(this.obj2, obj, null);
                    }
                }
            }
            private void Update_ViewModel_OrderDetails_IsEnabled(global::System.Boolean obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Order\OrderView.xaml line 18
                    if (!isobj3IsEnabledDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Control_IsEnabled(this.obj3, obj);
                    }
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class OrderView_obj1_BindingsTracking
            {
                private global::System.WeakReference<OrderView_obj1_Bindings> weakRefToBindingObj; 

                public OrderView_obj1_BindingsTracking(OrderView_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<OrderView_obj1_Bindings>(obj);
                }

                public OrderView_obj1_Bindings TryGetBindingObject()
                {
                    OrderView_obj1_Bindings bindingObject = null;
                    if (weakRefToBindingObj != null)
                    {
                        weakRefToBindingObj.TryGetTarget(out bindingObject);
                        if (bindingObject == null)
                        {
                            weakRefToBindingObj = null;
                            ReleaseAllListeners();
                        }
                    }
                    return bindingObject;
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_ViewModel(null);
                    UpdateChildListeners_ViewModel_OrderDetails(null);
                }

                public void PropertyChanged_ViewModel(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    OrderView_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::Inventory.ViewModels.OrderDetailsWithItemsViewModel obj = sender as global::Inventory.ViewModels.OrderDetailsWithItemsViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_ViewModel_OrderDetails(obj.OrderDetails, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "OrderDetails":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_OrderDetails(obj.OrderDetails, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::Inventory.ViewModels.OrderDetailsWithItemsViewModel cache_ViewModel = null;
                public void UpdateChildListeners_ViewModel(global::Inventory.ViewModels.OrderDetailsWithItemsViewModel obj)
                {
                    if (obj != cache_ViewModel)
                    {
                        if (cache_ViewModel != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_ViewModel).PropertyChanged -= PropertyChanged_ViewModel;
                            cache_ViewModel = null;
                        }
                        if (obj != null)
                        {
                            cache_ViewModel = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_ViewModel;
                        }
                    }
                }
                public void PropertyChanged_ViewModel_OrderDetails(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    OrderView_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::Inventory.ViewModels.OrderDetailsViewModel obj = sender as global::Inventory.ViewModels.OrderDetailsViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_ViewModel_OrderDetails_Title(obj.Title, DATA_CHANGED);
                                bindings.Update_ViewModel_OrderDetails_IsEnabled(obj.IsEnabled, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "Title":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_OrderDetails_Title(obj.Title, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "IsEnabled":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_OrderDetails_IsEnabled(obj.IsEnabled, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::Inventory.ViewModels.OrderDetailsViewModel cache_ViewModel_OrderDetails = null;
                public void UpdateChildListeners_ViewModel_OrderDetails(global::Inventory.ViewModels.OrderDetailsViewModel obj)
                {
                    if (obj != cache_ViewModel_OrderDetails)
                    {
                        if (cache_ViewModel_OrderDetails != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_ViewModel_OrderDetails).PropertyChanged -= PropertyChanged_ViewModel_OrderDetails;
                            cache_ViewModel_OrderDetails = null;
                        }
                        if (obj != null)
                        {
                            cache_ViewModel_OrderDetails = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_ViewModel_OrderDetails;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 4: // Views\Order\OrderView.xaml line 20
                {
                    this.details = (global::Inventory.Views.OrderDetails)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // Views\Order\OrderView.xaml line 1
                {                    
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    OrderView_obj1_Bindings bindings = new OrderView_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                    global::Windows.UI.Xaml.Markup.XamlBindingHelper.SetDataTemplateComponent(element1, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

