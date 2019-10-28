using System;
using System.Windows.Input;
using Inventory.Common;
using Inventory.ViewModels;
using Linphone;
using Linphone.Model;
using Windows.Devices.Enumeration;
using Windows.Media.Devices;

namespace Inventory.Services
{
    public class AudioParametersViewModel : ViewModelBase
    {
        public AudioParametersViewModel(ICommonServices commonServices) : base(commonServices)
        {
            ReadLinphoneSoundDevice();
            ReadSoundDevice();
        }

        public ICommand SaveAudioParametrsCommand => new RelayCommand(OnSaveAudioParametrs);

        private bool _echocancellation;
        public bool Echocancellation
        {
            get { return _echocancellation; }
            set { Set(ref _echocancellation, value); }
        }

        private DeviceInformationCollection _speakersDevices;
        public DeviceInformationCollection SpeakersDevices
        {
            get { return _speakersDevices; }
            set { Set(ref _speakersDevices, value); }
        }
        private string _speakerDeviceId;
        public string SpeakerDeviceId
        {
            get { return _speakerDeviceId; }
            set { Set(ref _speakerDeviceId, value); }
        }

        private DeviceInformationCollection _microphoneDevices;
        public DeviceInformationCollection MicrophoneDevices
        {
            get { return _microphoneDevices; }
            set { Set(ref _microphoneDevices, value); }
        }
        private string _microphoneDeviceId;
        public string MicrophoneDeviceId
        {
            get { return _microphoneDeviceId; }
            set { Set(ref _microphoneDeviceId, value); }
        }

        private DeviceInformationCollection _ringerDevices;
        public DeviceInformationCollection RingerDevices
        {
            get { return _ringerDevices; }
            set { Set(ref _ringerDevices, value); }
        }
        private string _ringerDeviceId;
        public string RingerDeviceId
        {
            get { return _ringerDeviceId; }
            set { Set(ref _ringerDeviceId, value); }
        }

        private string _linphoneSpeakerDeviceName = "";
        private string _linphoneMicrophoneDeviceName = "";
        private string _linphoneRingerDeviceName = "";

        private void OnSaveAudioParametrs()
        {
            //LinphoneManager.Instance.Core.Config.SetString("sound", "playback_dev_id", "WASAPI: " + SpeakerDeviceId);
            //LinphoneManager.Instance.Core.Config.SetString("sound", "capture_dev_id", "WASAPI: " + MicrophoneDeviceId);
            ////LinphoneManager.Instance.Core.Config.SetString("sound", "capture_dev_id", "WASAPI: " + "Microphone (A4TECH USB2.0 Camera (Audio))");
            //LinphoneManager.Instance.Core.Config.SetString("sound", "ringer_dev_id", "WASAPI: " + RingerDeviceId);
            //LinphoneManager.Instance.Core.Config.SetString("sound", "playback_gain_db", "1.0");
            //LinphoneManager.Instance.Core.Config.SetString("sound", "mic_gain_db", "1.0");
            //LinphoneManager.Instance.Core.Config.SetString("sound", "echocancellation", (Echocancellation ? 1 : 0).ToString());
            //LinphoneManager.Instance.Core.Config.SetString("sound", "ec_filter", "MSWebRTCAEC");
            //LinphoneManager.Instance.Core.Config.Sync();
            //string ss=LinphoneManager.Instance.Core.Config.GetString("sound", "capture_dev_id", "WASAPI: " + MicrophoneDeviceId);
            Core lc = LinphoneManager.Instance.Core;
            //ProxyConfig _cfg = lc.DefaultProxyConfig;
            lc.PlaybackDevice = "WASAPI: " + SpeakerDeviceId;
            lc.CaptureDevice = "WASAPI: " + MicrophoneDeviceId; // Микрофон (A4TECH USB2.0 Camera (Audio))
            //lc.CaptureDevice = "WASAPI: Микрофон (Устройство с поддержкой High Definition Audio)";
            //lc.CaptureDevice = "WASAPI: " + "Микрофон(A4TECH USB2.0 Camera (Audio))";
            lc.RingerDevice = "WASAPI: " + RingerDeviceId;
            lc.PlaybackGainDb = 1.0F;
            lc.MicGainDb = 1.0F;
            lc.EchoCancellationEnabled = Echocancellation;
            lc.EchoCancellerFilterName = "MSWebRTCAEC";
            lc.ReloadSoundDevices();

            //LinphoneManager.Instance.SpeakerEnabled=true;
        }

        private void ReadLinphoneSoundDevice()
        {
            _linphoneMicrophoneDeviceName = LinphoneManager.Instance.Core.CaptureDevice;
            _linphoneRingerDeviceName = LinphoneManager.Instance.Core.RingerDevice;
            _linphoneSpeakerDeviceName = LinphoneManager.Instance.Core.PlaybackDevice;
            _echocancellation = LinphoneManager.Instance.Core.EchoCancellationEnabled;
        }

        private async void ReadSoundDevice()
        {
            AudioDeviceRole role = AudioDeviceRole.Default;
            string MicrophoneDeviceId_ = MediaDevice.GetDefaultAudioCaptureId(role);
            string SpeakerDeviceId_ = MediaDevice.GetDefaultAudioRenderId(role);
            string RingerDeviceId_ = MediaDevice.GetDefaultAudioRenderId(role);

            SpeakersDevices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());
            SpeakerDeviceId = "";
            foreach (var dev in SpeakersDevices)
            {
                if (_linphoneSpeakerDeviceName == "WASAPI: " + dev.Name) SpeakerDeviceId = dev.Name;
            }
            if (String.IsNullOrEmpty(SpeakerDeviceId))
            {
                foreach (var dev in SpeakersDevices)
                {
                    if (SpeakerDeviceId_ == dev.Id) SpeakerDeviceId = dev.Name;
                }
            }

            RingerDevices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());
            RingerDeviceId = "";
            foreach (var dev in RingerDevices)
            {
                if (_linphoneRingerDeviceName == "WASAPI: " + dev.Name) RingerDeviceId = dev.Name;
            }
            if (String.IsNullOrEmpty(RingerDeviceId))
                foreach (var dev in RingerDevices)
                {
                    {
                        if (RingerDeviceId_ == dev.Id) RingerDeviceId = dev.Name;
                    }
                }

            MicrophoneDevices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioCaptureSelector());
            MicrophoneDeviceId = "";
            foreach (var dev in MicrophoneDevices)
            {
                if (_linphoneMicrophoneDeviceName == "WASAPI: " + dev.Name) MicrophoneDeviceId = dev.Name;
            }
            if (String.IsNullOrEmpty(MicrophoneDeviceId))
            {
                foreach (var dev in MicrophoneDevices)
                {
                    if (MicrophoneDeviceId_ == dev.Id) MicrophoneDeviceId = dev.Name;
                }
            }
        }
    }
}
