﻿#pragma checksum "D:\Delivery\src\Inventory.App\Views\Dishes\DishesView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E4EB0AF72A6E766FC1EE8A59DE7ABD8C"
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
    partial class DishesView : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Inventory_Views_DishesList_ViewModel(global::Inventory.Views.DishesList obj, global::Inventory.ViewModels.DishListViewModel value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Inventory.ViewModels.DishListViewModel) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Inventory.ViewModels.DishListViewModel), targetNullValue);
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
            public static void Set_Inventory_Controls_Section_Header(global::Inventory.Controls.Section obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.Header = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_SplitView_IsPaneOpen(global::Windows.UI.Xaml.Controls.SplitView obj, global::System.Boolean value)
            {
                obj.IsPaneOpen = value;
            }
            public static void Set_Windows_UI_Xaml_UIElement_Visibility(global::Windows.UI.Xaml.UIElement obj, global::Windows.UI.Xaml.Visibility value)
            {
                obj.Visibility = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_TreeView_ItemsSource(global::Windows.UI.Xaml.Controls.TreeView obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_Grid_RowSpan(global::Windows.UI.Xaml.FrameworkElement obj, global::System.Int32 value)
            {
                global::Windows.UI.Xaml.Controls.Grid.SetRowSpan(obj, value);
            }
            public static void Set_Inventory_Controls_Section_IsButtonVisible(global::Inventory.Controls.Section obj, global::System.Boolean value)
            {
                obj.IsButtonVisible = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class DishesView_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IDataTemplateComponent,
            global::Windows.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IDishesView_Bindings
        {
            private global::Inventory.Views.DishesView dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private global::Windows.UI.Xaml.ResourceDictionary localResources;
            private global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement> converterLookupRoot;

            // Fields for each control that has bindings.
            private global::Inventory.Controls.WindowTitle obj3;
            private global::Windows.UI.Xaml.Controls.SplitView obj4;
            private global::Windows.UI.Xaml.Controls.Grid obj5;
            private global::Windows.UI.Xaml.Controls.TreeView obj8;
            private global::Windows.UI.Xaml.Controls.Grid obj10;
            private global::Inventory.Controls.Section obj11;
            private global::Inventory.Views.DishesList obj12;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj3TitleDisabled = false;
            private static bool isobj4IsPaneOpenDisabled = false;
            private static bool isobj5VisibilityDisabled = false;
            private static bool isobj8ItemsSourceDisabled = false;
            private static bool isobj10RowSpanDisabled = false;
            private static bool isobj11HeaderDisabled = false;
            private static bool isobj11IsButtonVisibleDisabled = false;
            private static bool isobj12ViewModelDisabled = false;

            private DishesView_obj1_BindingsTracking bindingsTracking;

            public DishesView_obj1_Bindings()
            {
                this.bindingsTracking = new DishesView_obj1_BindingsTracking(this);
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 30 && columnNumber == 50)
                {
                    isobj3TitleDisabled = true;
                }
                else if (lineNumber == 33 && columnNumber == 20)
                {
                    isobj4IsPaneOpenDisabled = true;
                }
                else if (lineNumber == 37 && columnNumber == 31)
                {
                    isobj5VisibilityDisabled = true;
                }
                else if (lineNumber == 44 && columnNumber == 74)
                {
                    isobj8ItemsSourceDisabled = true;
                }
                else if (lineNumber == 60 && columnNumber == 23)
                {
                    isobj10RowSpanDisabled = true;
                }
                else if (lineNumber == 61 && columnNumber == 39)
                {
                    isobj11HeaderDisabled = true;
                }
                else if (lineNumber == 64 && columnNumber == 31)
                {
                    isobj11IsButtonVisibleDisabled = true;
                }
                else if (lineNumber == 65 && columnNumber == 43)
                {
                    isobj12ViewModelDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 3: // Views\Dishes\DishesView.xaml line 30
                        this.obj3 = (global::Inventory.Controls.WindowTitle)target;
                        break;
                    case 4: // Views\Dishes\DishesView.xaml line 32
                        this.obj4 = (global::Windows.UI.Xaml.Controls.SplitView)target;
                        break;
                    case 5: // Views\Dishes\DishesView.xaml line 37
                        this.obj5 = (global::Windows.UI.Xaml.Controls.Grid)target;
                        break;
                    case 8: // Views\Dishes\DishesView.xaml line 44
                        this.obj8 = (global::Windows.UI.Xaml.Controls.TreeView)target;
                        break;
                    case 10: // Views\Dishes\DishesView.xaml line 60
                        this.obj10 = (global::Windows.UI.Xaml.Controls.Grid)target;
                        break;
                    case 11: // Views\Dishes\DishesView.xaml line 61
                        this.obj11 = (global::Inventory.Controls.Section)target;
                        break;
                    case 12: // Views\Dishes\DishesView.xaml line 65
                        this.obj12 = (global::Inventory.Views.DishesList)target;
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

            // IDishesView_Bindings

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
                    this.dataRoot = (global::Inventory.Views.DishesView)newDataRoot;
                    return true;
                }
                return false;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }
            public void SetConverterLookupRoot(global::Windows.UI.Xaml.FrameworkElement rootElement)
            {
                this.converterLookupRoot = new global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement>(rootElement);
            }

            public global::Windows.UI.Xaml.Data.IValueConverter LookupConverter(string key)
            {
                if (this.localResources == null)
                {
                    global::Windows.UI.Xaml.FrameworkElement rootElement;
                    this.converterLookupRoot.TryGetTarget(out rootElement);
                    this.localResources = rootElement.Resources;
                    this.converterLookupRoot = null;
                }
                return (global::Windows.UI.Xaml.Data.IValueConverter) (this.localResources.ContainsKey(key) ? this.localResources[key] : global::Windows.UI.Xaml.Application.Current.Resources[key]);
            }

            private bool TryGet_ViewModel_DishList_IsMultipleSelection(out global::System.Boolean val)
            {
                global::Inventory.ViewModels.DishListViewModel obj;
                if (TryGet_ViewModel_DishList(out obj) && obj != null)
                {
                    val = obj.IsMultipleSelection;
                    return true;
                }
                else
                {
                    val = default(global::System.Boolean);
                    return false;
                }
            }

            private bool TryGet_ViewModel_DishList(out global::Inventory.ViewModels.DishListViewModel val)
            {
                global::Inventory.ViewModels.DishesViewModel obj;
                if (TryGet_ViewModel(out obj) && obj != null)
                {
                    val = obj.DishList;
                    return true;
                }
                else
                {
                    val = default(global::Inventory.ViewModels.DishListViewModel);
                    return false;
                }
            }

            private bool TryGet_ViewModel(out global::Inventory.ViewModels.DishesViewModel val)
            {
                global::Inventory.Views.DishesView obj;
                if (TryGet_(out obj) && obj != null)
                {
                    val = obj.ViewModel;
                    return true;
                }
                else
                {
                    val = default(global::Inventory.ViewModels.DishesViewModel);
                    return false;
                }
            }

            private bool TryGet_(out global::Inventory.Views.DishesView val)
            {
                val = this.dataRoot;
                return true;
            }

            private delegate void InvokeFunctionDelegate(int phase);
            private global::System.Collections.Generic.Dictionary<string, InvokeFunctionDelegate> PendingFunctionBindings = new global::System.Collections.Generic.Dictionary<string, InvokeFunctionDelegate>();

            private void Invoke_M_GetRowSpan_2271599836(int phase)
            {
                global::System.Boolean p0;
                if (!TryGet_ViewModel_DishList_IsMultipleSelection(out p0)) { return; }
                global::System.Int32 result = this.dataRoot.GetRowSpan(p0);
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Dishes\DishesView.xaml line 60
                    if (!isobj10RowSpanDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Grid_RowSpan(this.obj10, result);
                    }
                }
            }

            private void CompleteUpdate(int phase)
            {
                foreach(var function in this.PendingFunctionBindings)
                {
                    function.Value.Invoke(phase);
                }
                this.PendingFunctionBindings.Clear();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::Inventory.Views.DishesView obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel(obj.ViewModel, phase);
                        this.Update_M_GetRowSpan_2271599836(phase);
                    }
                }
                this.CompleteUpdate(phase);
            }
            private void Update_ViewModel(global::Inventory.ViewModels.DishesViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ViewModel(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_DishList(obj.DishList, phase);
                        this.Update_ViewModel_IsFilterPanelOpen(obj.IsFilterPanelOpen, phase);
                    }
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_CategoryTree(obj.CategoryTree, phase);
                        this.Update_ViewModel_IsMainView(obj.IsMainView, phase);
                    }
                }
            }
            private void Update_ViewModel_DishList(global::Inventory.ViewModels.DishListViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ViewModel_DishList(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_DishList_Title(obj.Title, phase);
                        this.Update_ViewModel_DishList_IsMultipleSelection(obj.IsMultipleSelection, phase);
                    }
                }
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // Views\Dishes\DishesView.xaml line 65
                    if (!isobj12ViewModelDisabled)
                    {
                        XamlBindingSetters.Set_Inventory_Views_DishesList_ViewModel(this.obj12, obj, null);
                    }
                }
            }
            private void Update_ViewModel_DishList_Title(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Dishes\DishesView.xaml line 30
                    if (!isobj3TitleDisabled)
                    {
                        XamlBindingSetters.Set_Inventory_Controls_WindowTitle_Title(this.obj3, obj, null);
                    }
                    // Views\Dishes\DishesView.xaml line 61
                    if (!isobj11HeaderDisabled)
                    {
                        XamlBindingSetters.Set_Inventory_Controls_Section_Header(this.obj11, obj, null);
                    }
                }
            }
            private void Update_ViewModel_IsFilterPanelOpen(global::System.Boolean obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Dishes\DishesView.xaml line 32
                    if (!isobj4IsPaneOpenDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_SplitView_IsPaneOpen(this.obj4, obj);
                    }
                    // Views\Dishes\DishesView.xaml line 37
                    if (!isobj5VisibilityDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj5, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("BoolToVisibilityConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                    }
                }
            }
            private void Update_ViewModel_CategoryTree(global::System.Collections.Generic.List<global::Inventory.Models.MenuFolderTreeModel> obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // Views\Dishes\DishesView.xaml line 44
                    if (!isobj8ItemsSourceDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TreeView_ItemsSource(this.obj8, obj, null);
                    }
                }
            }
            private void Update_ViewModel_DishList_IsMultipleSelection(global::System.Boolean obj, int phase)
            {
                this.Update_M_GetRowSpan_2271599836(phase);
            }
            private void Update_M_GetRowSpan_2271599836(int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    if (!isobj10RowSpanDisabled)
                    {
                        this.PendingFunctionBindings["M_GetRowSpan_2271599836"] = new InvokeFunctionDelegate(this.Invoke_M_GetRowSpan_2271599836); 
                    }
                }
            }
            private void Update_ViewModel_IsMainView(global::System.Boolean obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // Views\Dishes\DishesView.xaml line 61
                    if (!isobj11IsButtonVisibleDisabled)
                    {
                        XamlBindingSetters.Set_Inventory_Controls_Section_IsButtonVisible(this.obj11, obj);
                    }
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class DishesView_obj1_BindingsTracking
            {
                private global::System.WeakReference<DishesView_obj1_Bindings> weakRefToBindingObj; 

                public DishesView_obj1_BindingsTracking(DishesView_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<DishesView_obj1_Bindings>(obj);
                }

                public DishesView_obj1_Bindings TryGetBindingObject()
                {
                    DishesView_obj1_Bindings bindingObject = null;
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
                    UpdateChildListeners_ViewModel_DishList(null);
                }

                public void PropertyChanged_ViewModel(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    DishesView_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::Inventory.ViewModels.DishesViewModel obj = sender as global::Inventory.ViewModels.DishesViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_ViewModel_DishList(obj.DishList, DATA_CHANGED);
                                bindings.Update_ViewModel_IsFilterPanelOpen(obj.IsFilterPanelOpen, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "DishList":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_DishList(obj.DishList, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "IsFilterPanelOpen":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_IsFilterPanelOpen(obj.IsFilterPanelOpen, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                        bindings.CompleteUpdate(DATA_CHANGED);
                    }
                }
                private global::Inventory.ViewModels.DishesViewModel cache_ViewModel = null;
                public void UpdateChildListeners_ViewModel(global::Inventory.ViewModels.DishesViewModel obj)
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
                public void PropertyChanged_ViewModel_DishList(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    DishesView_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::Inventory.ViewModels.DishListViewModel obj = sender as global::Inventory.ViewModels.DishListViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_ViewModel_DishList_Title(obj.Title, DATA_CHANGED);
                                bindings.Update_ViewModel_DishList_IsMultipleSelection(obj.IsMultipleSelection, DATA_CHANGED);
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
                                        bindings.Update_ViewModel_DishList_Title(obj.Title, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "IsMultipleSelection":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_DishList_IsMultipleSelection(obj.IsMultipleSelection, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                        bindings.CompleteUpdate(DATA_CHANGED);
                    }
                }
                private global::Inventory.ViewModels.DishListViewModel cache_ViewModel_DishList = null;
                public void UpdateChildListeners_ViewModel_DishList(global::Inventory.ViewModels.DishListViewModel obj)
                {
                    if (obj != cache_ViewModel_DishList)
                    {
                        if (cache_ViewModel_DishList != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_ViewModel_DishList).PropertyChanged -= PropertyChanged_ViewModel_DishList;
                            cache_ViewModel_DishList = null;
                        }
                        if (obj != null)
                        {
                            cache_ViewModel_DishList = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_ViewModel_DishList;
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
            case 4: // Views\Dishes\DishesView.xaml line 32
                {
                    this.splitView = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 6: // Views\Dishes\DishesView.xaml line 54
                {
                    this.btnToogleFilterPanel = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.btnToogleFilterPanel).Click += this.BtnToogleFilterPanel_Click;
                }
                break;
            case 7: // Views\Dishes\DishesView.xaml line 42
                {
                    this.PaneHeader = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8: // Views\Dishes\DishesView.xaml line 44
                {
                    this.TreeCategory = (global::Windows.UI.Xaml.Controls.TreeView)(target);
                    ((global::Windows.UI.Xaml.Controls.TreeView)this.TreeCategory).ItemInvoked += this.TreeCategory_ItemInvoked;
                }
                break;
            case 11: // Views\Dishes\DishesView.xaml line 61
                {
                    global::Inventory.Controls.Section element11 = (global::Inventory.Controls.Section)(target);
                    ((global::Inventory.Controls.Section)element11).HeaderButtonClick += this.OpenInNewView;
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
            case 1: // Views\Dishes\DishesView.xaml line 1
                {                    
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    DishesView_obj1_Bindings bindings = new DishesView_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    bindings.SetConverterLookupRoot(this);
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

