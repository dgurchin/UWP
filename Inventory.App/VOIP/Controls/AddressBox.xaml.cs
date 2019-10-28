﻿/*
AddressBox.xaml.cs
Copyright (C) 2016  Belledonne Communications, Grenoble, France
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Linphone.VOIP.Controls
{

    public partial class AddressBox : UserControl
    {
        public String Text
        {
            get { return address.Text; }
            set {
                address.Text = value;
                if(value.Length > 0)
                {
                    Backspace.IsEnabled = true;
                } else
                {
                    Backspace.IsEnabled = false;
                }
            }
        }

        public AddressBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void backspace_Hold_1(object sender, RoutedEventArgs e)
        {
            address.Text = "";
        }

        private void backspace_Click_1(object sender, RoutedEventArgs e)
        {
            if (address.Text.Length > 0)
                address.Text = address.Text.Substring(0, address.Text.Length - 1);

        }

        private void address_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(address.Text.Length > 0)
            {
                Backspace.IsEnabled = true;
            } else
            {
                Backspace.IsEnabled = false;
            }
        }
    }
}
