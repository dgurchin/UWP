﻿/*
VideoSettings.xaml.cs
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
using Linphone.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Linphone.VOIP.Views {
    /// <summary>
    /// Page displaying the video settings and the video codecs to let the user enable/disable them.
    /// </summary>
    public partial class VideoSettings : Page {
        private CallSettingsManager _callSettings = new CallSettingsManager();
        private CodecsSettingsManager _codecsSettings = new CodecsSettingsManager();
        private bool saveSettingsOnLeave = true;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public VideoSettings() {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += back_Click;

            _callSettings.Load();
            VideoEnabled.IsOn = (bool)_callSettings.VideoEnabled;
            AutomaticallyInitiateVideo.IsOn = (bool)_callSettings.AutomaticallyInitiateVideo;
            AutomaticallyAcceptVideo.IsOn = (bool)_callSettings.AutomaticallyAcceptVideo;
            SelfViewEnabled.IsOn = (bool)_callSettings.SelfViewEnabled;

            List<string> videoSizes = new List<string>
            {
                "vga",
                "qvga"
            };
            PreferredVideoSize.ItemsSource = videoSizes;
            PreferredVideoSize.SelectedItem = (_callSettings.PreferredVideoSize != null) ? _callSettings.PreferredVideoSize : "vga";

            _codecsSettings.Load();
            foreach (PayloadType p in LinphoneManager.Instance.Core.VideoPayloadTypes) {
                if (p.MimeType.Equals("H264")) {
                    H264.Visibility = Visibility.Visible;
                }
            }
            H264.IsOn = _codecsSettings.H264;
            VP8.IsOn = _codecsSettings.VP8;
        }

        /// <summary>
        /// Method called when the user is navigation away from this page
        /// </summary>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            if (saveSettingsOnLeave) {
                Save();
            }
            base.OnNavigatingFrom(e);
        }

        private void Save() {
            _codecsSettings.H264 = ToBool(H264.IsOn);
            _codecsSettings.VP8 = ToBool(VP8.IsOn);
            _codecsSettings.Save();

            _callSettings.VideoEnabled = ToBool(VideoEnabled.IsOn);
            _callSettings.AutomaticallyInitiateVideo = AutomaticallyInitiateVideo.IsOn;
            _callSettings.AutomaticallyAcceptVideo = AutomaticallyAcceptVideo.IsOn;
            _callSettings.SelfViewEnabled = ToBool(SelfViewEnabled.IsOn);
            _callSettings.PreferredVideoSize = (PreferredVideoSize.SelectedItem != null) ? PreferredVideoSize.SelectedItem.ToString() : "vga";
            _callSettings.Save();
        }

        private void cancel_Click_1(object sender, EventArgs e) {
            saveSettingsOnLeave = false;
            if (Frame.CanGoBack) {
                Frame.GoBack();
            }
        }

        private bool ToBool(bool? enabled) {
            if (!enabled.HasValue)
                enabled = false;
            return (bool)enabled;
        }

        private void save_Click_1(object sender, EventArgs e) {
            if (Frame.CanGoBack) {
                Frame.GoBack();
            }
        }

        private void back_Click(object sender, BackRequestedEventArgs e) {
            e.Handled = true;
            if (Frame.CanGoBack) {
                Frame.GoBack();
            }
        }
    }
}