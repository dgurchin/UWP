using System;
using System.Collections.Generic;
using System.Windows.Input;
using Inventory.Common;
using Inventory.Data;
using Inventory.Models;
using Inventory.ViewModels;
using Linphone;
using Linphone.Model;
using Windows.Devices.Enumeration;
using Windows.Media.Devices;

namespace Inventory.Services
{
    public class AudioCodec : ObservableObject
    {
        public int Channels { get; set; }
        public int ClockRate { get; set; }
        public string Description { get; set; }
        public string EncoderDescription { get; set; }
        public bool IsUsable { get; set; }
        public bool IsVbr { get; set; }
        public string MimeType { get; set; }
        public int NormalBitrate { get; set; }
        public int Number { get; set; }
        public string RecvFmtp { get; set; }
        public string SendFmtp { get; set; }
        public int Type { get; set; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { Set(ref _isEnabled, value); }
        }
    }


    public class AudioCodecsViewModel : ViewModelBase
    {
        public AudioCodecsViewModel(ICommonServices commonServices) : base(commonServices)
        {
            ReadLinphoneCodecs();
        }
        public ICommand SaveAudioCodecsCommand => new RelayCommand(OnSaveAudioCodecs);

        private IEnumerable<PayloadType> _audioPayloadTypes;
        public List<AudioCodec> _audioCodec = new List<AudioCodec>();

        private void ReadLinphoneCodecs()
        {
            _audioPayloadTypes = LinphoneManager.Instance.Core.AudioPayloadTypes;
            foreach (var row in _audioPayloadTypes)
            {
                AudioCodec codec = new AudioCodec();
                codec.Channels = row.Channels;
                codec.ClockRate = row.ClockRate;
                codec.Description = row.Description;
                codec.EncoderDescription = row.EncoderDescription;
                codec.IsVbr = row.IsVbr;
                codec.MimeType = row.MimeType;
                codec.NormalBitrate = row.NormalBitrate;
                codec.Number = row.Number;
                codec.RecvFmtp = row.RecvFmtp;
                codec.SendFmtp = row.SendFmtp;
                codec.Type = row.Type;
                codec.IsEnabled = row.Enabled();
                //codec.IsEnabled = true;
                if (codec.SendFmtp == null) codec.SendFmtp = "";
                if (codec.RecvFmtp == null) codec.RecvFmtp = "";
                _audioCodec.Add(codec);
            }
        }
        private void OnSaveAudioCodecs()
        {
            int i = 0;
            _audioPayloadTypes = LinphoneManager.Instance.Core.AudioPayloadTypes;
            foreach (var row in _audioPayloadTypes)
            {
                row.Enable(_audioCodec[i++].IsEnabled);
            }
        }

    }
}
