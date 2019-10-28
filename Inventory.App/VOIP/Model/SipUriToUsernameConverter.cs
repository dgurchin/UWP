﻿/*
SipUriToUsernameConverter.cs
Copyright (C) 2015  Belledonne Communications, Grenoble, France
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

using Linphone;
using System;
using Windows.UI.Xaml.Data;

namespace Linphone.Model {
    public class SipUriToUsernameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            string sipAddress = (string)value;
            Address addr = LinphoneManager.Instance.Core.InterpretUrl(sipAddress);
            return addr.Username;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
