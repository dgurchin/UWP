﻿#pragma checksum "D:\Delivery\src\Inventory.App\VOIP\Views\Settings.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D001D748A130C6C278E9AF169D26B850"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Linphone.VOIP.Views
{
    partial class Settings : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // VOIP\Views\Settings.xaml line 13
                {
                    this.LayoutRoot = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // VOIP\Views\Settings.xaml line 27
                {
                    global::Windows.UI.Xaml.Controls.ListViewItem element3 = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)element3).Tapped += this.account_Click_1;
                }
                break;
            case 4: // VOIP\Views\Settings.xaml line 41
                {
                    global::Windows.UI.Xaml.Controls.ListViewItem element4 = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)element4).Tapped += this.audio_Click_1;
                }
                break;
            case 5: // VOIP\Views\Settings.xaml line 55
                {
                    global::Windows.UI.Xaml.Controls.ListViewItem element5 = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)element5).Tapped += this.video_Click_1;
                }
                break;
            case 6: // VOIP\Views\Settings.xaml line 69
                {
                    global::Windows.UI.Xaml.Controls.ListViewItem element6 = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)element6).Tapped += this.advanced_Click_1;
                }
                break;
            case 7: // VOIP\Views\Settings.xaml line 83
                {
                    global::Windows.UI.Xaml.Controls.ListViewItem element7 = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)element7).Tapped += this.LockScreenSettings_Click_1;
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
            return returnValue;
        }
    }
}

