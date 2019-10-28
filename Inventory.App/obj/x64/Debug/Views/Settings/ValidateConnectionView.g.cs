﻿#pragma checksum "D:\Delivery\src\Inventory.App\Views\Settings\ValidateConnectionView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DFF5780B25471392CA2F7FDF860665F0"
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
    partial class ValidateConnectionView : 
        global::Windows.UI.Xaml.Controls.ContentDialog, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_ContentDialog_PrimaryButtonText(global::Windows.UI.Xaml.Controls.ContentDialog obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.PrimaryButtonText = value ?? global::System.String.Empty;
            }
            public static void Set_Windows_UI_Xaml_Controls_ContentDialog_SecondaryButtonText(global::Windows.UI.Xaml.Controls.ContentDialog obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.SecondaryButtonText = value ?? global::System.String.Empty;
            }
            public static void Set_Windows_UI_Xaml_UIElement_Visibility(global::Windows.UI.Xaml.UIElement obj, global::Windows.UI.Xaml.Visibility value)
            {
                obj.Visibility = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Text(global::Windows.UI.Xaml.Controls.TextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class ValidateConnectionView_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IDataTemplateComponent,
            global::Windows.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IValidateConnectionView_Bindings
        {
            private global::Inventory.Views.ValidateConnectionView dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private global::Windows.UI.Xaml.ResourceDictionary localResources;
            private global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement> converterLookupRoot;

            // Fields for each control that has bindings.
            private global::System.WeakReference obj1;
            private global::Windows.UI.Xaml.Controls.StackPanel obj2;
            private global::Windows.UI.Xaml.Controls.TextBlock obj3;
            private global::Windows.UI.Xaml.Controls.TextBlock obj4;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj1PrimaryButtonTextDisabled = false;
            private static bool isobj1SecondaryButtonTextDisabled = false;
            private static bool isobj2VisibilityDisabled = false;
            private static bool isobj3TextDisabled = false;
            private static bool isobj3VisibilityDisabled = false;
            private static bool isobj4TextDisabled = false;

            private ValidateConnectionView_obj1_BindingsTracking bindingsTracking;

            public ValidateConnectionView_obj1_Bindings()
            {
                this.bindingsTracking = new ValidateConnectionView_obj1_BindingsTracking(this);
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 8 && columnNumber == 5)
                {
                    isobj1PrimaryButtonTextDisabled = true;
                }
                else if (lineNumber == 10 && columnNumber == 5)
                {
                    isobj1SecondaryButtonTextDisabled = true;
                }
                else if (lineNumber == 15 && columnNumber == 34)
                {
                    isobj2VisibilityDisabled = true;
                }
                else if (lineNumber == 19 && columnNumber == 20)
                {
                    isobj3TextDisabled = true;
                }
                else if (lineNumber == 19 && columnNumber == 67)
                {
                    isobj3VisibilityDisabled = true;
                }
                else if (lineNumber == 16 && columnNumber == 24)
                {
                    isobj4TextDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 1: // Views\Settings\ValidateConnectionView.xaml line 1
                        this.obj1 = new global::System.WeakReference((global::Windows.UI.Xaml.Controls.ContentDialog)target);
                        break;
                    case 2: // Views\Settings\ValidateConnectionView.xaml line 15
                        this.obj2 = (global::Windows.UI.Xaml.Controls.StackPanel)target;
                        break;
                    case 3: // Views\Settings\ValidateConnectionView.xaml line 19
                        this.obj3 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 4: // Views\Settings\ValidateConnectionView.xaml line 16
                        this.obj4 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
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

            // IValidateConnectionView_Bindings

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
                    this.dataRoot = (global::Inventory.Views.ValidateConnectionView)newDataRoot;
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

            // Update methods for each path node used in binding steps.
            private void Update_(global::Inventory.Views.ValidateConnectionView obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel(obj.ViewModel, phase);
                    }
                }
            }
            private void Update_ViewModel(global::Inventory.ViewModels.ValidateConnectionViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ViewModel(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_PrimaryButtonText(obj.PrimaryButtonText, phase);
                        this.Update_ViewModel_SecondaryButtonText(obj.SecondaryButtonText, phase);
                        this.Update_ViewModel_HasMessage(obj.HasMessage, phase);
                        this.Update_ViewModel_Message(obj.Message, phase);
                        this.Update_ViewModel_ProgressStatus(obj.ProgressStatus, phase);
                    }
                }
            }
            private void Update_ViewModel_PrimaryButtonText(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Settings\ValidateConnectionView.xaml line 1
                    if (!isobj1PrimaryButtonTextDisabled)
                    {
                        if ((this.obj1.Target as global::Windows.UI.Xaml.Controls.ContentDialog) != null)
                        {
                            XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ContentDialog_PrimaryButtonText((this.obj1.Target as global::Windows.UI.Xaml.Controls.ContentDialog), obj, null);
                        }
                    }
                }
            }
            private void Update_ViewModel_SecondaryButtonText(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Settings\ValidateConnectionView.xaml line 1
                    if (!isobj1SecondaryButtonTextDisabled)
                    {
                        if ((this.obj1.Target as global::Windows.UI.Xaml.Controls.ContentDialog) != null)
                        {
                            XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ContentDialog_SecondaryButtonText((this.obj1.Target as global::Windows.UI.Xaml.Controls.ContentDialog), obj, null);
                        }
                    }
                }
            }
            private void Update_ViewModel_HasMessage(global::System.Boolean obj, int phase)
            {
                if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                {
                    this.Update_ViewModel_HasMessage_Cast_HasMessage_To_Visibility(obj ? global::Windows.UI.Xaml.Visibility.Visible : global::Windows.UI.Xaml.Visibility.Collapsed, phase);
                }
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Settings\ValidateConnectionView.xaml line 15
                    if (!isobj2VisibilityDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj2, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("InverseBoolToVisibilityConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                    }
                }
            }
            private void Update_ViewModel_Message(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Settings\ValidateConnectionView.xaml line 19
                    if (!isobj3TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj3, obj, null);
                    }
                }
            }
            private void Update_ViewModel_HasMessage_Cast_HasMessage_To_Visibility(global::Windows.UI.Xaml.Visibility obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Settings\ValidateConnectionView.xaml line 19
                    if (!isobj3VisibilityDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj3, obj);
                    }
                }
            }
            private void Update_ViewModel_ProgressStatus(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Settings\ValidateConnectionView.xaml line 16
                    if (!isobj4TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj4, obj, null);
                    }
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class ValidateConnectionView_obj1_BindingsTracking
            {
                private global::System.WeakReference<ValidateConnectionView_obj1_Bindings> weakRefToBindingObj; 

                public ValidateConnectionView_obj1_BindingsTracking(ValidateConnectionView_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<ValidateConnectionView_obj1_Bindings>(obj);
                }

                public ValidateConnectionView_obj1_Bindings TryGetBindingObject()
                {
                    ValidateConnectionView_obj1_Bindings bindingObject = null;
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
                }

                public void PropertyChanged_ViewModel(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    ValidateConnectionView_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::Inventory.ViewModels.ValidateConnectionViewModel obj = sender as global::Inventory.ViewModels.ValidateConnectionViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_ViewModel_PrimaryButtonText(obj.PrimaryButtonText, DATA_CHANGED);
                                bindings.Update_ViewModel_SecondaryButtonText(obj.SecondaryButtonText, DATA_CHANGED);
                                bindings.Update_ViewModel_HasMessage(obj.HasMessage, DATA_CHANGED);
                                bindings.Update_ViewModel_Message(obj.Message, DATA_CHANGED);
                                bindings.Update_ViewModel_ProgressStatus(obj.ProgressStatus, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "PrimaryButtonText":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_PrimaryButtonText(obj.PrimaryButtonText, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "SecondaryButtonText":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_SecondaryButtonText(obj.SecondaryButtonText, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "HasMessage":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_HasMessage(obj.HasMessage, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "Message":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_Message(obj.Message, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "ProgressStatus":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_ProgressStatus(obj.ProgressStatus, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::Inventory.ViewModels.ValidateConnectionViewModel cache_ViewModel = null;
                public void UpdateChildListeners_ViewModel(global::Inventory.ViewModels.ValidateConnectionViewModel obj)
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
            case 1: // Views\Settings\ValidateConnectionView.xaml line 1
                {
                    global::Windows.UI.Xaml.Controls.ContentDialog element1 = (global::Windows.UI.Xaml.Controls.ContentDialog)(target);
                    ((global::Windows.UI.Xaml.Controls.ContentDialog)element1).PrimaryButtonClick += this.OnOkClick;
                    ((global::Windows.UI.Xaml.Controls.ContentDialog)element1).SecondaryButtonClick += this.OnCancelClick;
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
            case 1: // Views\Settings\ValidateConnectionView.xaml line 1
                {                    
                    global::Windows.UI.Xaml.Controls.ContentDialog element1 = (global::Windows.UI.Xaml.Controls.ContentDialog)target;
                    ValidateConnectionView_obj1_Bindings bindings = new ValidateConnectionView_obj1_Bindings();
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

