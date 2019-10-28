using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;

using Inventory.Views;
using Inventory.ViewModels;
using Linphone.Model;
using Linphone;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;

namespace Inventory.Services
{
    public class LinphoneManagerService : CallControllerListener
    {
        private Dictionary<TransportType, string> EnumToTransport;
        private Dictionary<string, TransportType> TransportToEnum;
        public bool acceptCall { get; set; }

        public LinphoneManagerService()
        {
            EnumToTransport = new Dictionary<TransportType, string>()
             {
                { TransportType.Udp,  ResourceLoader.GetForCurrentView().GetString("TransportUDP") },
                { TransportType.Tcp, ResourceLoader.GetForCurrentView().GetString("TransportTCP") },
                { TransportType.Tls, ResourceLoader.GetForCurrentView().GetString("TransportTLS") }
            };

            TransportToEnum = new Dictionary<string, TransportType>()
            {
                { ResourceLoader.GetForCurrentView().GetString("TransportUDP"), TransportType.Udp },
                { ResourceLoader.GetForCurrentView().GetString("TransportTCP"), TransportType.Tcp },
                { ResourceLoader.GetForCurrentView().GetString("TransportTLS"), TransportType.Tls }
            };
        }


        public String sipAddress { get; set; }
        private static INavigationService _navigationService = null;

        public Dictionary<String, String> DictSIP { get; set; }
        public Dictionary<String, String> ChangesDictSIP { get; set; }

        public static INavigationService NavigationService
        {
            get
            {
                return _navigationService;
            }
        }
        private static LinphoneManagerService _instance = new LinphoneManagerService();
        public static LinphoneManagerService Instance
        {
            get
            {
                return _instance;
            }
        }
        public RegistrationState state { get; set; }
        //private int unreadMessageCount;
        //public int UnreadMessageCount
        //{
        //    get
        //    {
        //        return unreadMessageCount;
        //    }

        //    set
        //    {
        //        unreadMessageCount = value;
        //    }
        //}
        private int missedCallCount;
        public int MissedCallCount
        {
            get
            {
                return missedCallCount;
            }

            set
            {
                missedCallCount = value;
            }
        }

        public int LinphoneManagerInit(CoreDispatcher dispatcher)
        {
            _navigationService = ServiceLocator.Current.GetService<INavigationService>();
            SettingsManager.InstallConfigFile();
            acceptCall = false;
            LinphoneManager.Instance.InitLinphoneCore();
            LinphoneManager.Instance.CallListener = this;
            LinphoneManager.Instance.CoreDispatcher = dispatcher;
            //LinphoneManager.Instance.RegistrationChanged += RegistrationChanged;
            //LinphoneManager.Instance.MessageReceived += MessageReceived;
            LinphoneManager.Instance.CallStateChangedEvent += CallStateChanged;
            return 0;
        }
        //private void RegistrationChanged(ProxyConfig config, RegistrationState state, string message)
        //{
        //    RefreshStatus();
        //}
        //public void RefreshStatus()
        //{
        //    if (LinphoneManager.Instance.Core.DefaultProxyConfig == null)
        //        state = RegistrationState.None;
        //    else
        //        state = LinphoneManager.Instance.Core.DefaultProxyConfig.State;
        //}
        //private void MessageReceived(ChatRoom room, ChatMessage message)
        //{

        //    UnreadMessageCount = LinphoneManager.Instance.GetUnreadMessageCount();
        //}
        public void CallStateChanged(Call call, CallState state)
        {
            MissedCallCount = LinphoneManager.Instance.Core.MissedCallsCount;
            //if (call == null)
            //    return;
        }

        public void NewCallStarted(string callerNumber)
        {
            List<String> parameters = new List<String>();
            parameters.Add(callerNumber);
            //((CallControllerListener)Instance).NewCallStarted(callerNumber);
        }

        public void CallEnded(Call call)
        {
            //((CallControllerListener)Instance).CallEnded(call);
        }

        public void CallIncoming(Call call)
        {
            string phoneNumber = call.RemoteAddress.AsString();
            string SIP_ = call.RemoteAddress.AsStringUriOnly();
             phoneNumber = SIP_.Split(":")[1].Split("@")[0];
            PhoneCall(phoneNumber);
        }
        public async void PhoneCall(string phoneNumber)
        {
            var ret = await NavigationService.CreateNewViewAsync<PhoneCallReceiveViewModel>(new PhoneCallArgs { PhoneNumber = phoneNumber });
        }

        public void MuteStateChanged(bool isMicMuted)
        {
            //((CallControllerListener)Instance).MuteStateChanged(isMicMuted);
        }

        public void PauseStateChanged(Call call, bool isCallPaused, bool isCallPausedByRemote)
        {
            //((CallControllerListener)Instance).PauseStateChanged(call, isCallPaused, isCallPausedByRemote);
        }

        public void CallUpdatedByRemote(Call call, bool isVideoAdded)
        {
            //((CallControllerListener)Instance).CallUpdatedByRemote(call, isVideoAdded);
        }


        #region Настройки учетной записи SIP SIP
        /// <summary>
        /// Загрузка настроек учетной записи SIP.
        /// </summary>
        #region Constants settings names
        private const string UsernameKeyName = "Username";
        private const string UserIdKeyName = "UserId";
        private const string PasswordKeyName = "Password";
        private const string DomainKeyName = "Domain";
        private const string ProxyKeyName = "Proxy";
        private const string OutboundProxyKeyName = "OutboundProxy";
        private const string DisplayNameKeyName = "DisplayName";
        private const string TransportKeyName = "Transport";
        private const string ExpireKeyName = "Expire";
        private const string AVPFKeyName = "AVPF";
        private const string Ice = "ICE";

        #endregion
        public void Load()
        {
            DictSIP = new Dictionary<string, string>();
            DictSIP.Add(UsernameKeyName,"");                        
            DictSIP.Add(UserIdKeyName,"");                          
            DictSIP.Add(PasswordKeyName,"");                        
            DictSIP.Add(DisplayNameKeyName,"");                    
            DictSIP.Add(DomainKeyName,"");                          
            DictSIP.Add(ProxyKeyName,"");                           
            DictSIP.Add(OutboundProxyKeyName, false.ToString());    
            DictSIP.Add(TransportKeyName,"UDP");                    
            DictSIP.Add(ExpireKeyName,"");                          
            DictSIP.Add(AVPFKeyName, false.ToString());            
            DictSIP.Add(Ice, false.ToString());                     

            ProxyConfig cfg = LinphoneManager.Instance.Core.DefaultProxyConfig;
            if (cfg != null)
            {
                Address address = cfg.IdentityAddress;
                if (address != null)
                {
                    Address proxyAddress = LinphoneManager.Instance.Core.CreateAddress(cfg.ServerAddr);
                    DictSIP[ProxyKeyName] = proxyAddress.AsStringUriOnly();
                    DictSIP[TransportKeyName] = EnumToTransport[proxyAddress.Transport];
                    DictSIP[UsernameKeyName] = address.Username;
                    DictSIP[DomainKeyName] = address.Domain;
                    DictSIP[OutboundProxyKeyName] = (cfg.Route != null && cfg.Route.Length > 0).ToString();
                    DictSIP[ExpireKeyName] = String.Format("{0}", cfg.Expires);
                    AuthInfo authInfo = LinphoneManager.Instance.Core.FindAuthInfo(address.Domain, address.Username, address.Domain);
                    if (authInfo != null)
                    {
                        DictSIP[PasswordKeyName] = authInfo.Passwd;
                        DictSIP[UserIdKeyName] = authInfo.Userid;
                    }
                    if (cfg.NatPolicy == null) cfg.NatPolicy = LinphoneManager.Instance.Core.CreateNatPolicy();
                    DictSIP[Ice] = cfg.NatPolicy.IceEnabled.ToString();
                    DictSIP[DisplayNameKeyName] = address.DisplayName;
                    DictSIP[AVPFKeyName] = (cfg.AvpfMode == AVPFMode.Enabled) ? true.ToString() : false.ToString();
                }
            }
            ChangesDictSIP = new Dictionary<string, string>();
            //ChangesDictSIP[UsernameKeyName] = "";
        }

        /// <summary>
        /// Сохранение учетной записи SIP.
        /// </summary>
        public void Save()
        {
            bool AccountChanged = ValueChanged(UsernameKeyName) || ValueChanged(UserIdKeyName) || ValueChanged(PasswordKeyName) || ValueChanged(DomainKeyName)
                || ValueChanged(ProxyKeyName) || ValueChanged(OutboundProxyKeyName) || ValueChanged(DisplayNameKeyName) || ValueChanged(TransportKeyName) || ValueChanged(ExpireKeyName);
            if (AccountChanged)
            {
                Core lc = LinphoneManager.Instance.Core;
                ProxyConfig cfg = lc.DefaultProxyConfig;
                if (cfg != null)
                {
                    cfg.Edit();
                    cfg.RegisterEnabled = false;
                    cfg.Done();

                    //Wait for unregister to complete
                    int timeout = 2000;
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (true)
                    {
                        if (stopwatch.ElapsedMilliseconds >= timeout || cfg.State == RegistrationState.Cleared || cfg.State == RegistrationState.None)
                        {
                            break;
                        }
                        LinphoneManager.Instance.Core.Iterate();
                        System.Threading.Tasks.Task.Delay(100);
                    }
                }

                String username = GetNew(UsernameKeyName);
                String userid = GetNew(UserIdKeyName);
                String password = GetNew(PasswordKeyName);
                String domain = GetNew(DomainKeyName);
                String proxy = GetNew(ProxyKeyName);
                String displayname = GetNew(DisplayNameKeyName);
                String transport = GetNew(TransportKeyName);
                String expires = GetNew(ExpireKeyName);
                bool avpf = Convert.ToBoolean(GetNew(AVPFKeyName));
                bool ice = Convert.ToBoolean(GetNew(Ice));

                bool outboundProxy = Convert.ToBoolean(GetNew(OutboundProxyKeyName));
                lc.ClearAllAuthInfo();
                lc.ClearProxyConfig();
                if ((username != null) && (username.Length > 0) && (domain != null) && (domain.Length > 0))
                {
                    cfg = lc.CreateProxyConfig();
                    cfg.Edit();
                    if (displayname != null && displayname.Length > 0)
                    {
                        cfg.IdentityAddress = Factory.Instance.CreateAddress("\"" + displayname + "\" " + "<sip:" + username + "@" + domain + ">");
                    }
                    else
                    {
                        cfg.IdentityAddress = Factory.Instance.CreateAddress("<sip:" + username + "@" + domain + ">");
                    }
                    if ((proxy == null) || (proxy.Length <= 0))
                    {
                        proxy = "sip:" + domain;
                    }
                    else
                    {
                        if (!proxy.StartsWith("sip:") && !proxy.StartsWith("<sip:")
                            && !proxy.StartsWith("sips:") && !proxy.StartsWith("<sips:"))
                        {
                            proxy = "sip:" + proxy;
                        }
                    }


                    cfg.ServerAddr = proxy;

                    if (transport != null)
                    {
                        Address proxyAddr = LinphoneManager.Instance.Core.CreateAddress(proxy);
                        if (proxyAddr != null)
                        {
                            proxyAddr.Transport = TransportToEnum[transport];
                            cfg.ServerAddr = proxyAddr.AsStringUriOnly();
                        }
                    }
                    if (outboundProxy)
                    {
                        cfg.Route = cfg.ServerAddr;
                    }

                    if (cfg.NatPolicy == null)
                        cfg.NatPolicy = cfg.Core.CreateNatPolicy();

                    if (cfg.NatPolicy != null)
                        cfg.NatPolicy.IceEnabled = ice;

                    int result = 0;
                    int.TryParse(expires, out result);
                    if (result != 0)
                    {
                        cfg.Expires = result;
                    }

                    var auth = lc.CreateAuthInfo(username, userid, password, "", "", domain);
                    lc.AddAuthInfo(auth);
                    lc.CaptureDevice = "WASAPI: Microphone (High Definition Audio Device)";
                    lc.AddProxyConfig(cfg);
                    lc.DefaultProxyConfig = cfg;
                    //lc.CaptureDevice = "WASAPI: Microphone (High Definition Audio Device)";
                    //LinphoneManager.Instance.AddPushInformationsToContactParams();
                    cfg.AvpfMode = (avpf) ? AVPFMode.Enabled : AVPFMode.Disabled;
                    cfg.RegisterEnabled = true;
                    cfg.Done();
               
                }
            }
        }

        public void Delete()
        {
            ProxyConfig cfg = LinphoneManager.Instance.Core.DefaultProxyConfig;
            if (cfg != null)
            {
                LinphoneManager.Instance.Core.ClearProxyConfig();
                LinphoneManager.Instance.Core.ClearAllAuthInfo();
            }
        }

        protected bool ValueChanged(String Key)
        {
            return DictSIP.ContainsKey(Key);
        }

        protected String GetNew(String Key)
        {
            if (ChangesDictSIP.ContainsKey(Key))
            {
                return ChangesDictSIP[Key];
            }
            else if (ChangesDictSIP.ContainsKey(Key))
            {
                return ChangesDictSIP[Key];
            }
            return null;
        }
        #endregion
    }
}
