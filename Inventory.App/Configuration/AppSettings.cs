#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.IO;

using Integra.LoadingMonitor.DataProviders;
using Integra.LoadingMonitor.Web.DataProviders;

using Windows.ApplicationModel;
using Windows.Storage;

namespace Inventory
{
    public class AppSettings
    {
        #region Private fields
        const string DB_NAME = "Delivery";
        const string DB_VERSION = "3.4.0";
        //const string DB_BASEURL = "https://vanarsdelinventory.blob.core.windows.net/database";
        const string DB_BASEURL = "/Assest/SQLiteBase";
        #endregion

        #region Ctor
        static AppSettings()
        {
            Current = new AppSettings();
        }
        #endregion

        #region Properties
        static public AppSettings Current { get; }

        static public readonly string AppLogPath = "AppLog";
        static public readonly string AppLogName = $"AppLog.1.0.db";
        static public readonly string AppLogFileName = Path.Combine(AppLogPath, AppLogName);

        public readonly string AppLogConnectionString = $"Data Source={AppLogFileName}";

        static public readonly string DatabasePath = "Database";
        static public readonly string DatabaseName = $"{DB_NAME}.2.02.db";
        static public readonly string DatabasePattern = $"{DB_NAME}.2.02.pattern.db";
        static public readonly string DatabaseFileName = Path.Combine(DatabasePath, DatabaseName);
        static public readonly string DatabasePatternFileName = Path.Combine(DatabasePath, DatabasePattern);
        static public readonly string DatabaseUrl = $"{DB_BASEURL}/{DatabaseName}";

        public readonly string SQLiteConnectionString = $"Data Source={DatabaseFileName}";

        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;
        #endregion

        #region MainApp
        public string Version
        {
            get
            {
                var ver = Package.Current.Id.Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
            }
        }

        public string DbVersion => DB_VERSION;

        public string UserName
        {
            get => GetSettingsValue("UserName", default(String));
            set => LocalSettings.Values["UserName"] = value;
        }

        public string WindowsHelloPublicKeyHint
        {
            get => GetSettingsValue("WindowsHelloPublicKeyHint", default(String));
            set => LocalSettings.Values["WindowsHelloPublicKeyHint"] = value;
        }

        public Services.DataProviderType DataProvider
        {
            get => (Services.DataProviderType)GetSettingsValue("DataProvider", (int)Services.DataProviderType.SQLServer);
            set => LocalSettings.Values["DataProvider"] = (int)value;
        }

        public string SQLServerConnectionString
        {
            get => GetSettingsValue("SQLServerConnectionString", @"Data Source=localhost;Initial Catalog=DeliveryPro;Integrated Security=SSPI");
            set => SetSettingsValue("SQLServerConnectionString", value);
        }

        public bool IsRandomErrorsEnabled
        {
            get => GetSettingsValue("IsRandomErrorsEnabled", false);
            set => LocalSettings.Values["IsRandomErrorsEnabled"] = value;
        }
        #endregion

        #region Sip
        public string SipPhoneName
        {
            get => GetSettingsValue("SipPhoneName", "901");
            set => LocalSettings.Values["SipPhoneName"] = value;
        }

        public string SipPhonePassword
        {
            get => GetSettingsValue("SipPhonePassword", "27403ed03477008c369c75ac498fd74d");
            set => LocalSettings.Values["SipPhonePassword"] = value;
        }

        public string SipPhoneDomain
        {
            get => GetSettingsValue("SipPhoneDomain", "10.0.0.101");
            set => LocalSettings.Values["SipPhoneDomain"] = value;
        }

        public string SipPhoneTransportProtocol
        {
            get => GetSettingsValue("SipPhoneTransportProtocol", "UDP");
            set => LocalSettings.Values["SipPhoneTransportProtocol"] = value;
        }

        public string SipPhonePort
        {
            get => GetSettingsValue("SipPhonePort", "5160");
            set => LocalSettings.Values["SipPhonePort"] = value;
        }

        public bool ClientPhoneVisibility
        {
            get => GetSettingsValue("ClientPhoneVisibility", true);
            set => LocalSettings.Values["ClientPhoneVisibility"] = value;
        }
        #endregion

        #region LoadingMonitor

        public string MonitorHost
        {
            get => GetSettingsValue("MonitorHost", "http://localhost:4242");
            set => LocalSettings.Values["MonitorHost"] = value;
        }

        public string MonitorUserName
        {
            get => GetSettingsValue("MonitorUserName", "test");
            set => LocalSettings.Values["MonitorUserName"] = value;
        }

        public string MonitorPassword
        {
            get => GetSettingsValue("MonitorPassword", "test");
            set => LocalSettings.Values["MonitorPassword"] = value;
        }

        /// <summary>
        /// Get Loading Monitor data provider by settings
        /// </summary>
        /// <returns></returns>
        public IDataProvider GetMonitorDataProvider()
        {
            IDataProvider dataProvider = new KDSWebServiceDataProvider(
                Current.MonitorHost, 
                Current.MonitorUserName, 
                Current.MonitorPassword);
            return dataProvider;
        }
        #endregion

        #region Private methods
        private TResult GetSettingsValue<TResult>(string name, TResult defaultValue)
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = defaultValue;
                }
                return (TResult)LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }

        private void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value;
        }
        #endregion
    }
}
