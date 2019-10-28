using System;
using System.Threading;
using System.Windows.Input;

using Inventory.Common;
using Inventory.ViewModels;

using Linphone;
using Linphone.Model;

using Windows.ApplicationModel.Resources;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace Inventory.Services
{
    #region PhoneCallArgs
    public class PhoneCallArgs
    {
        static public PhoneCallArgs CreateEmpty() => new PhoneCallArgs { isAceptCall = false };
        public PhoneCallArgs()
        {
            PhoneNumber = "";
        }
        public string PhoneNumber { get; set; }
        public bool isAceptCall { get; set; }
        public bool IsEmpty { get; set; }
    }
    #endregion
    public class PhoneCallViewModel : ViewModelBase
    {
        public PhoneCallArgs ViewModelArgs { get; private set; }
        public CoreDispatcher CoreDispatcher { get; set; }
        public int ThreadIdMyPage { get; set; }
        private ThreadPoolTimer _threadPoolTimer;

        #region Brushes
        private SolidColorBrush _lightYellow = new SolidColorBrush(Colors.LightYellow);
        private SolidColorBrush _green = new SolidColorBrush(Colors.Green);
        private SolidColorBrush _gray = new SolidColorBrush(Colors.Gray);
        private SolidColorBrush _red = new SolidColorBrush(Colors.Red);
        #endregion

        private RegistrationState _registrationState;
        private float _sliderVolumeSound = 1;
        

        public float SliderVolumeSound
        {
            get { return _sliderVolumeSound; }
            set { Set(ref _sliderVolumeSound, value); OnSliderVolumeSound(); }
        }
        private float _sliderVolumeMicrophone;
        public float SliderVolumeMicrophone
        {
            get { return _sliderVolumeMicrophone; }
            set { Set(ref _sliderVolumeMicrophone, value); OnSliderVolumeMicrophone(); }
        }
        private bool _phoneActive;
        public bool PhoneActive
        {
            get { return _phoneActive; }
            set { Set(ref _phoneActive, value); }
        }
        private bool _notIncommingCall=true;
        public bool NotIncommingCall
        {
            get { return _notIncommingCall; }
            set { Set(ref _notIncommingCall, value); }
        }

        private bool _phoneVisible = true;
        public bool PhoneVisible
        {
            get { return _phoneVisible; }
            set { Set(ref _phoneVisible, value); }
        }

        private int _timeRegistrationState;
        private SolidColorBrush _microphoneColor = new SolidColorBrush(Colors.Green);
        public SolidColorBrush MicrophoneColor
        {
            get { return _microphoneColor; }
            set { Set(ref _microphoneColor, value); }
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { Set(ref _phoneNumber, value);}
        }
        private bool _clockNot;
        public bool ClockNot
        {
            get { return _clockNot; }
            set { Set(ref _clockNot, value); }
        }
        private bool _clockWaitIncoming;
        public bool ClockWaitIncoming
        {
            get { return _clockWaitIncoming; }
            set { Set(ref _clockWaitIncoming, value); }
        }
        private bool _clockWaitOutgoing;
        public bool ClockWaitOutgoing
        {
            get { return _clockWaitOutgoing; }
            set { Set(ref _clockWaitOutgoing, value); }
        }
        private bool _clockRunning;
        public bool ClockRunning
        {
            get { return _clockRunning; }
            set { Set(ref _clockRunning, value); }
        }
        private string _clockRunningText = "00:00:00";
        public string ClockRunningText
        {
            get { return _clockRunningText; }
            set
            {
                Set(ref _clockRunningText, value);
            }
        }
        private SolidColorBrush _statusTexteColor = new SolidColorBrush(Colors.Green);
        public SolidColorBrush StatusTextColor
        {
            get { return _statusTexteColor; }
            set { Set(ref _statusTexteColor, value); }
        }
        private string _statusText = "";
        public string StatusText
        {
            get { return _statusText; }
            set
            {
                Set(ref _statusText, value);
            }
        }

        public ISettingsService SettingsService { get; }

        public PhoneCallViewModel(ICommonServices commonServices, ISettingsService settingsService) : base(commonServices)
        {
            SettingsService = settingsService;
            LinphoneManager.Instance.RegistrationChanged += RegistrationChanged;
            LinphoneManager.Instance.CallStateChangedEvent += CallStateChanged;
            LinphoneManager.Instance.Core.RefreshRegisters();
            //SliderVolumeMicrophone = LinphoneManager.Instance.Core.MicGainDb;
            //SliderVolumeSound = LinphoneManager.Instance.Core.PlaybackGainDb;
        }

        public void Load()
        {
            timerStart();
            ThreadIdMyPage = Thread.CurrentThread.ManagedThreadId;
            if (LinphoneManager.Instance.Core.CallsNb > 0)
            {
                CallStateChanged(GetCurrentCall(), CallState.Connected);
            }
            RefreshStatus();
        }

        public void Unload()
        {
            timerStop();
        }

        protected void OnCloseCurrentView()
        {
            Unload();
            NavigationService.CloseViewAsync();
        }

        #region RelayCommand
        public ICommand CallPhoneAccept => new RelayCommand(OnCallPhoneAccept);
        public ICommand CallPhoneReject => new RelayCommand(OnCallPhoneReject);
        public ICommand CallPhoneEnd => new RelayCommand(OnCallPhoneEnd);
        public ICommand CallphoneStart => new RelayCommand(OnCallphoneStart);
        public ICommand MicrophoneOnOff => new RelayCommand(OnMicrophoneOnOff);



        /// <summary>
        ///Принять звонок
        /// </summary>
        //private async void OnCallPhoneAccept()
        private void OnCallPhoneAccept()
        {
            ContextService.NewWindowID = 10;
            if (LinphoneManager.Instance.Core.CurrentCall != null)
            {
                LinphoneManager.Instance.Core.AcceptCall(LinphoneManager.Instance.Core.CurrentCall);
                OnSliderInit();
                ClockRunning = true;
            }
        }

        /// <summary>
        ///Отклонить звонок
        /// </summary>
//        private async void OnCallPhoneReject()
        private void OnCallPhoneReject()
        {
            LinphoneManager.Instance.EndCurrentCall();
            ClockRunning = false;
            OnCloseCurrentView();
        }

        /// <summary>
        ///Закончить разговор
        /// </summary>
        private void OnCallPhoneEnd()
        {
            LinphoneManager.Instance.EndCurrentCall();
            ClockRunning = false;
        }

        /// <summary>
        ///Позвонить
        /// </summary>
        private void OnCallphoneStart()
        {
            if (!(string.IsNullOrWhiteSpace(PhoneNumber)))
            {
                LinphoneManager.Instance.NewOutgoingCall(PhoneNumber);
                OnSliderInit();
                ClockRunning = true;
                ClockRunningText = "00" + ":" + "00" + ":" + "00";
            }
        }

        /// <summary>
        ///Включение/Выключение микрофона
        /// </summary>
        private void OnMicrophoneOnOff()
        {
            if (LinphoneManager.Instance.Core.MicEnabled)
            {
                LinphoneManager.Instance.Core.MicEnabled = false;
                MicrophoneColor = _gray;
            }
            else
            {
                LinphoneManager.Instance.Core.MicEnabled = true;
                MicrophoneColor = _green;
            }
        }
        /// <summary>
        ///Громкость наушников
        /// </summary>
        private void OnSliderVolumeSound()
        {
            LinphoneManager.Instance.Core.PlaybackGainDb = SliderVolumeSound;
        } 
        /// <summary>
        /// Чувствительность микрофона
        /// </summary>
        private void OnSliderVolumeMicrophone()
        {
            LinphoneManager.Instance.Core.MicGainDb = SliderVolumeMicrophone;
        }
        private void OnSliderInit()
        {
            SliderVolumeMicrophone = LinphoneManager.Instance.Core.MicGainDb;
            SliderVolumeSound = LinphoneManager.Instance.Core.PlaybackGainDb;
        }
        #endregion

        #region LinphoneManager делегаты + ... 
        private void RegistrationChanged(ProxyConfig config, RegistrationState state, string message)
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (ThreadIdMyPage != currentThreadId) return;
            RefreshStatus();
        }
        public void CallStateChanged(Call call, CallState state)
        {
            if (call == null) return;
            if (state == CallState.Connected)
            {
            }
            else if (state == CallState.End)
            {
            }
        }
        public Call GetCurrentCall()
        {
            Call call = LinphoneManager.Instance.Core.CurrentCall;
            if (call == null)
            {
                if (LinphoneManager.Instance.Core.CallsNb > 0)
                {
                    call = (Call)LinphoneManager.Instance.Core.Calls.GetEnumerator().Current;
                }
            }
            return call;
        }
        public void RefreshStatus()
        {
           _timeRegistrationState = 0;
            try
            {
                RegistrationState state;
                if (LinphoneManager.Instance.Core.DefaultProxyConfig == null)
                    state = RegistrationState.None;
                else
                    state = LinphoneManager.Instance.Core.DefaultProxyConfig.State;
                RefreshStatus(state);
            }
            catch (Exception ex)
            {
                // При Обновлении.
                LogException("Phone", "RefreshStatus", ex);
            }
        }
        public void RefreshStatus(RegistrationState state)
        {
            if (_registrationState!=state)
            {
                _registrationState = state;
            }
            if (state == RegistrationState.Ok) PhoneActive = true; else PhoneActive = false;
            if (state == RegistrationState.Ok)
            {
                StatusText = ResourceLoader.GetForCurrentView().GetString("Registered");
                StatusTextColor = _green;
            }
            else if (state == RegistrationState.Progress)
            {
                StatusText = ResourceLoader.GetForCurrentView().GetString("RegistrationInProgress");
                StatusTextColor = _lightYellow;
            }
            else if (state == RegistrationState.Failed)
            {
                if (LinphoneManager.Instance.Core.DefaultProxyConfig.Error == Reason.Forbidden)
                {
                    StatusText = ResourceLoader.GetForCurrentView().GetString("RegistrationFailedForbidden");
                    StatusTextColor = _red;
                }
                else
                {
                    StatusText = ResourceLoader.GetForCurrentView().GetString("RegistrationFailed");
                    StatusTextColor = _red;
                }
            }
            else
            {
                StatusText = ResourceLoader.GetForCurrentView().GetString("Disconnected");
                StatusTextColor = _lightYellow;
            }

        }
         #endregion

        #region Timer
        public void timerStart()
        {
            TimeSpan period = TimeSpan.FromMilliseconds(1000);
            if (CoreDispatcher != null)
            {
                if (_threadPoolTimer == null)
                {
                    _threadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
                    {
                        CoreDispatcher.RunIdleAsync((args) =>
                        {
                            timerTick();
                        });
                    }, period);
                }
            }
        }
        public void timerStop()
        {
            if (_threadPoolTimer != null)
            {
                _threadPoolTimer.Cancel();
                if (_threadPoolTimer != null) _threadPoolTimer = null;
            }
        }
        private void timerTick()
        {
            if ((_timeRegistrationState++>30)
                |(!PhoneActive))
            {
              //LinphoneManager.Instance.Core.RefreshRegisters();
                //_timeRegistrationState = 0;
              RefreshStatus();
            }
            if (!PhoneActive) return;
            //IReadOnlyList<CoreApplicationView> views = CoreApplication.Views;
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (ThreadIdMyPage != currentThreadId) return;
            Call call = GetCurrentCall();
            if (call == null)
            {
                ClockNot = true;
                ClockWaitIncoming = false;
                ClockRunning = false;
                return;
            }
            if (ThreadIdMyPage == currentThreadId)
            {
                TimeSpan callDuration = new TimeSpan(call.Duration * TimeSpan.TicksPerSecond);
                var hh = callDuration.Hours;
                var ss = callDuration.Seconds;
                var mm = callDuration.Minutes;
                ClockNot = false;
                if (hh > 0 | mm > 0 | ss > 0)
                {
                    ClockRunning = true;
                    ClockWaitIncoming = false;
                    if (MicrophoneColor == null)
                    {
                        if (LinphoneManager.Instance.Core.MicEnabled)
                        {
                            MicrophoneColor = _gray;
                        }
                        else
                        {
                            MicrophoneColor = _green;
                        }
                    }
                }
                else
                {
                    ClockRunning = false;
                    if (call.Dir == CallDir.Outgoing)
                    {
                        ClockWaitIncoming = false;
                        ClockWaitOutgoing = true;
                        ClockRunning = true;
                    }
                    else
                    {
                        ClockWaitIncoming = true;
                        ClockWaitOutgoing = false;
                        ClockRunning = false;
                    }
                }
                ClockRunningText = hh.ToString("00") + ":" + mm.ToString("00") + ":" + ss.ToString("00");
            }
        }
        #endregion
    }
}

