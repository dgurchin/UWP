﻿#pragma checksum "D:\Delivery\src\Inventory.App\Views\Customers\Details\CustomersCard.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CDEF8A859B768AF315817A1795397984"
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
    partial class CustomersCard : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Text(global::Windows.UI.Xaml.Controls.TextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
            public static void Set_Windows_UI_Xaml_Controls_PersonPicture_ProfilePicture(global::Windows.UI.Xaml.Controls.PersonPicture obj, global::Windows.UI.Xaml.Media.ImageSource value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.ImageSource) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.ImageSource), targetNullValue);
                }
                obj.ProfilePicture = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_PersonPicture_Initials(global::Windows.UI.Xaml.Controls.PersonPicture obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Initials = value ?? global::System.String.Empty;
            }
            public static void Set_Windows_UI_Xaml_Controls_Primitives_ButtonBase_Command(global::Windows.UI.Xaml.Controls.Primitives.ButtonBase obj, global::System.Windows.Input.ICommand value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Windows.Input.ICommand) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Windows.Input.ICommand), targetNullValue);
                }
                obj.Command = value;
            }
            public static void Set_Windows_UI_Xaml_UIElement_Visibility(global::Windows.UI.Xaml.UIElement obj, global::Windows.UI.Xaml.Visibility value)
            {
                obj.Visibility = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_Image_Source(global::Windows.UI.Xaml.Controls.Image obj, global::Windows.UI.Xaml.Media.ImageSource value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Windows.UI.Xaml.Media.ImageSource) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Windows.UI.Xaml.Media.ImageSource), targetNullValue);
                }
                obj.Source = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class CustomersCard_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IDataTemplateComponent,
            global::Windows.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            ICustomersCard_Bindings
        {
            private global::Inventory.Views.CustomersCard dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private global::Windows.UI.Xaml.ResourceDictionary localResources;
            private global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement> converterLookupRoot;

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.TextBlock obj2;
            private global::Windows.UI.Xaml.Controls.TextBlock obj3;
            private global::Windows.UI.Xaml.Controls.TextBlock obj4;
            private global::Windows.UI.Xaml.Controls.PersonPicture obj5;
            private global::Inventory.Controls.RoundButton obj6;
            private global::Windows.UI.Xaml.Controls.TextBlock obj7;
            private global::Windows.UI.Xaml.Controls.TextBlock obj8;
            private global::Windows.UI.Xaml.Controls.Image obj9;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj2TextDisabled = false;
            private static bool isobj3TextDisabled = false;
            private static bool isobj4TextDisabled = false;
            private static bool isobj5ProfilePictureDisabled = false;
            private static bool isobj5InitialsDisabled = false;
            private static bool isobj6CommandDisabled = false;
            private static bool isobj6VisibilityDisabled = false;
            private static bool isobj7TextDisabled = false;
            private static bool isobj8VisibilityDisabled = false;
            private static bool isobj9SourceDisabled = false;
            private static bool isobj9VisibilityDisabled = false;

            private CustomersCard_obj1_BindingsTracking bindingsTracking;

            public CustomersCard_obj1_Bindings()
            {
                this.bindingsTracking = new CustomersCard_obj1_BindingsTracking(this);
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 53 && columnNumber == 58)
                {
                    isobj2TextDisabled = true;
                }
                else if (lineNumber == 56 && columnNumber == 58)
                {
                    isobj3TextDisabled = true;
                }
                else if (lineNumber == 59 && columnNumber == 58)
                {
                    isobj4TextDisabled = true;
                }
                else if (lineNumber == 18 && columnNumber == 32)
                {
                    isobj5ProfilePictureDisabled = true;
                }
                else if (lineNumber == 19 && columnNumber == 32)
                {
                    isobj5InitialsDisabled = true;
                }
                else if (lineNumber == 26 && columnNumber == 39)
                {
                    isobj6CommandDisabled = true;
                }
                else if (lineNumber == 28 && columnNumber == 39)
                {
                    isobj6VisibilityDisabled = true;
                }
                else if (lineNumber == 47 && columnNumber == 24)
                {
                    isobj7TextDisabled = true;
                }
                else if (lineNumber == 35 && columnNumber == 36)
                {
                    isobj8VisibilityDisabled = true;
                }
                else if (lineNumber == 36 && columnNumber == 32)
                {
                    isobj9SourceDisabled = true;
                }
                else if (lineNumber == 40 && columnNumber == 32)
                {
                    isobj9VisibilityDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 2: // Views\Customers\Details\CustomersCard.xaml line 53
                        this.obj2 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 3: // Views\Customers\Details\CustomersCard.xaml line 56
                        this.obj3 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 4: // Views\Customers\Details\CustomersCard.xaml line 59
                        this.obj4 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 5: // Views\Customers\Details\CustomersCard.xaml line 17
                        this.obj5 = (global::Windows.UI.Xaml.Controls.PersonPicture)target;
                        break;
                    case 6: // Views\Customers\Details\CustomersCard.xaml line 22
                        this.obj6 = (global::Inventory.Controls.RoundButton)target;
                        break;
                    case 7: // Views\Customers\Details\CustomersCard.xaml line 44
                        this.obj7 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 8: // Views\Customers\Details\CustomersCard.xaml line 31
                        this.obj8 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 9: // Views\Customers\Details\CustomersCard.xaml line 36
                        this.obj9 = (global::Windows.UI.Xaml.Controls.Image)target;
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

            // ICustomersCard_Bindings

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
                    this.dataRoot = (global::Inventory.Views.CustomersCard)newDataRoot;
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
            private void Update_(global::Inventory.Views.CustomersCard obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_Item(obj.Item, phase);
                        this.Update_ViewModel(obj.ViewModel, phase);
                    }
                }
            }
            private void Update_Item(global::Inventory.Models.CustomerModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_Item(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_Item_Id(obj.Id, phase);
                        this.Update_Item_CreatedOn(obj.CreatedOn, phase);
                        this.Update_Item_LastModifiedOn(obj.LastModifiedOn, phase);
                        this.Update_Item_PictureSource(obj.PictureSource, phase);
                        this.Update_Item_Initials(obj.Initials, phase);
                        this.Update_Item_FullName(obj.FullName, phase);
                    }
                }
            }
            private void Update_Item_Id(global::System.Int32 obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 53
                    if (!isobj2TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj2, obj.ToString(), null);
                    }
                }
            }
            private void Update_Item_CreatedOn(global::System.DateTimeOffset obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 56
                    if (!isobj3TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj3, (global::System.String)this.LookupConverter("DateTimeOffsetToStringConverter").Convert(obj, typeof(global::System.String), null, null), null);
                    }
                }
            }
            private void Update_Item_LastModifiedOn(global::System.Nullable<global::System.DateTimeOffset> obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 59
                    if (!isobj4TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj4, (global::System.String)this.LookupConverter("DateTimeOffsetToStringConverter").Convert(obj, typeof(global::System.String), null, null), null);
                    }
                }
            }
            private void Update_Item_PictureSource(global::System.Object obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 17
                    if (!isobj5ProfilePictureDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_PersonPicture_ProfilePicture(this.obj5, (global::Windows.UI.Xaml.Media.ImageSource)this.LookupConverter("ObjectToImageConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Media.ImageSource), null, null), null);
                    }
                }
            }
            private void Update_Item_Initials(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 17
                    if (!isobj5InitialsDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_PersonPicture_Initials(this.obj5, obj, null);
                    }
                }
            }
            private void Update_ViewModel(global::Inventory.ViewModels.CustomerDetailsViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ViewModel(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_EditPictureCommand(obj.EditPictureCommand, phase);
                    }
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_IsEditMode(obj.IsEditMode, phase);
                        this.Update_ViewModel_IsEnabled(obj.IsEnabled, phase);
                        this.Update_ViewModel_NewPictureSource(obj.NewPictureSource, phase);
                    }
                }
            }
            private void Update_ViewModel_EditPictureCommand(global::System.Windows.Input.ICommand obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 22
                    if (!isobj6CommandDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Primitives_ButtonBase_Command(this.obj6, obj, null);
                    }
                }
            }
            private void Update_ViewModel_IsEditMode(global::System.Boolean obj, int phase)
            {
                if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                {
                    this.Update_ViewModel_IsEditMode_Cast_IsEditMode_To_Visibility(obj ? global::Windows.UI.Xaml.Visibility.Visible : global::Windows.UI.Xaml.Visibility.Collapsed, phase);
                }
            }
            private void Update_ViewModel_IsEditMode_Cast_IsEditMode_To_Visibility(global::Windows.UI.Xaml.Visibility obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 22
                    if (!isobj6VisibilityDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj6, obj);
                    }
                }
            }
            private void Update_Item_FullName(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 44
                    if (!isobj7TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj7, obj, null);
                    }
                }
            }
            private void Update_ViewModel_IsEnabled(global::System.Boolean obj, int phase)
            {
                if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                {
                    this.Update_ViewModel_IsEnabled_Cast_IsEnabled_To_Visibility(obj ? global::Windows.UI.Xaml.Visibility.Visible : global::Windows.UI.Xaml.Visibility.Collapsed, phase);
                }
            }
            private void Update_ViewModel_IsEnabled_Cast_IsEnabled_To_Visibility(global::Windows.UI.Xaml.Visibility obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 31
                    if (!isobj8VisibilityDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj8, obj);
                    }
                }
            }
            private void Update_ViewModel_NewPictureSource(global::System.Object obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\Customers\Details\CustomersCard.xaml line 36
                    if (!isobj9SourceDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Image_Source(this.obj9, (global::Windows.UI.Xaml.Media.ImageSource)this.LookupConverter("ObjectToImageConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Media.ImageSource), null, null), null);
                    }
                    // Views\Customers\Details\CustomersCard.xaml line 36
                    if (!isobj9VisibilityDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_UIElement_Visibility(this.obj9, (global::Windows.UI.Xaml.Visibility)this.LookupConverter("NullToVisibilityConverter").Convert(obj, typeof(global::Windows.UI.Xaml.Visibility), null, null));
                    }
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class CustomersCard_obj1_BindingsTracking
            {
                private global::System.WeakReference<CustomersCard_obj1_Bindings> weakRefToBindingObj; 

                public CustomersCard_obj1_BindingsTracking(CustomersCard_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<CustomersCard_obj1_Bindings>(obj);
                }

                public CustomersCard_obj1_Bindings TryGetBindingObject()
                {
                    CustomersCard_obj1_Bindings bindingObject = null;
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
                    UpdateChildListeners_(null);
                    UpdateChildListeners_Item(null);
                    UpdateChildListeners_ViewModel(null);
                }

                public void DependencyPropertyChanged_Item(global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop)
                {
                    CustomersCard_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        global::Inventory.Views.CustomersCard obj = sender as global::Inventory.Views.CustomersCard;
                        if (obj != null)
                        {
                            bindings.Update_Item(obj.Item, DATA_CHANGED);
                        }
                    }
                }
                public void DependencyPropertyChanged_ViewModel(global::Windows.UI.Xaml.DependencyObject sender, global::Windows.UI.Xaml.DependencyProperty prop)
                {
                    CustomersCard_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        global::Inventory.Views.CustomersCard obj = sender as global::Inventory.Views.CustomersCard;
                        if (obj != null)
                        {
                            bindings.Update_ViewModel(obj.ViewModel, DATA_CHANGED);
                        }
                    }
                }
                private long tokenDPC_Item = 0;
                private long tokenDPC_ViewModel = 0;
                public void UpdateChildListeners_(global::Inventory.Views.CustomersCard obj)
                {
                    CustomersCard_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        if (bindings.dataRoot != null)
                        {
                            bindings.dataRoot.UnregisterPropertyChangedCallback(global::Inventory.Views.CustomersCard.ItemProperty, tokenDPC_Item);
                            bindings.dataRoot.UnregisterPropertyChangedCallback(global::Inventory.Views.CustomersCard.ViewModelProperty, tokenDPC_ViewModel);
                        }
                        if (obj != null)
                        {
                            bindings.dataRoot = obj;
                            tokenDPC_Item = obj.RegisterPropertyChangedCallback(global::Inventory.Views.CustomersCard.ItemProperty, DependencyPropertyChanged_Item);
                            tokenDPC_ViewModel = obj.RegisterPropertyChangedCallback(global::Inventory.Views.CustomersCard.ViewModelProperty, DependencyPropertyChanged_ViewModel);
                        }
                    }
                }
                public void PropertyChanged_Item(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    CustomersCard_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::Inventory.Models.CustomerModel obj = sender as global::Inventory.Models.CustomerModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_Item_Id(obj.Id, DATA_CHANGED);
                                bindings.Update_Item_CreatedOn(obj.CreatedOn, DATA_CHANGED);
                                bindings.Update_Item_LastModifiedOn(obj.LastModifiedOn, DATA_CHANGED);
                                bindings.Update_Item_PictureSource(obj.PictureSource, DATA_CHANGED);
                                bindings.Update_Item_Initials(obj.Initials, DATA_CHANGED);
                                bindings.Update_Item_FullName(obj.FullName, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "Id":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_Item_Id(obj.Id, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "CreatedOn":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_Item_CreatedOn(obj.CreatedOn, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "LastModifiedOn":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_Item_LastModifiedOn(obj.LastModifiedOn, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "PictureSource":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_Item_PictureSource(obj.PictureSource, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "Initials":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_Item_Initials(obj.Initials, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "FullName":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_Item_FullName(obj.FullName, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::Inventory.Models.CustomerModel cache_Item = null;
                public void UpdateChildListeners_Item(global::Inventory.Models.CustomerModel obj)
                {
                    if (obj != cache_Item)
                    {
                        if (cache_Item != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_Item).PropertyChanged -= PropertyChanged_Item;
                            cache_Item = null;
                        }
                        if (obj != null)
                        {
                            cache_Item = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_Item;
                        }
                    }
                }
                public void PropertyChanged_ViewModel(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    CustomersCard_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::Inventory.ViewModels.CustomerDetailsViewModel obj = sender as global::Inventory.ViewModels.CustomerDetailsViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_ViewModel_IsEditMode(obj.IsEditMode, DATA_CHANGED);
                                bindings.Update_ViewModel_IsEnabled(obj.IsEnabled, DATA_CHANGED);
                                bindings.Update_ViewModel_NewPictureSource(obj.NewPictureSource, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "IsEditMode":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_IsEditMode(obj.IsEditMode, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "IsEnabled":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_IsEnabled(obj.IsEnabled, DATA_CHANGED);
                                    }
                                    break;
                                }
                                case "NewPictureSource":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_NewPictureSource(obj.NewPictureSource, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::Inventory.ViewModels.CustomerDetailsViewModel cache_ViewModel = null;
                public void UpdateChildListeners_ViewModel(global::Inventory.ViewModels.CustomerDetailsViewModel obj)
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
            case 1: // Views\Customers\Details\CustomersCard.xaml line 1
                {                    
                    global::Windows.UI.Xaml.Controls.UserControl element1 = (global::Windows.UI.Xaml.Controls.UserControl)target;
                    CustomersCard_obj1_Bindings bindings = new CustomersCard_obj1_Bindings();
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

