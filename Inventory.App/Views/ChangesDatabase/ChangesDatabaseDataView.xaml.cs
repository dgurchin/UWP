using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Inventory.Data;
using Inventory.Data.Services;
using Inventory.Services;
using Inventory.ViewModels;

using Linphone.Model;
using Linphone.VOIP.Views;

using Microsoft.EntityFrameworkCore;
using Windows.Devices.Enumeration;
using Windows.Media.Devices;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//using System.Management;
//using System.Management.Instrumentation;
//using System.IO.Ports;
//using System.Diagnostics;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Inventory.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangesDatabaseDataView : Page
    {
        public OrdersViewModel VM { get; }
        public INavigationService NavigationService { get; }
        public ILogService LogService { get; }
        public IDialogService DialogService { get; }

        public ChangesDatabaseDataView()
        {
            InitializeComponent();
            VM = ServiceLocator.Current.GetService<OrdersViewModel>();
            NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            DialogService = ServiceLocator.Current.GetService<IDialogService>();
            LogService = ServiceLocator.Current.GetService<ILogService>();
        }

        /// <summary>
        /// Заполнение полей поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecalcKeyFind_Click(object sender, RoutedEventArgs e)
        {
            //[Table("Orders")]
            //public partial class Order
            //public string BuildSearchTerms() => $"{OrderID} {CustomerID} {RestaurantCity} {RestaurantRegion}".ToLower();

            //[Table("Customers")]
            //public partial class Customer
            //public string BuildSearchTerms() => $"{CustomerID} {FirstName} {LastName} {EmailAddress} {AddressLine1}".ToLower();

            //[Table("Products")]
            //public partial class Product
            //public string BuildSearchTerms() => $"{ProductID} {Name} {Color}".ToLower();

            //SQLiteDataService dataServiceBase = new SQLiteDataService(AppSettings.Current.SQLiteConnectionString);       //: DataServiceBase
            //DataServiceFactory dataServiceFactory =new DataServiceFactory();                                             //: IDataServiceFactory
            //IDataService dataService = dataServiceFactory.CreateDataService();                                           //: IDataService


            SQLiteDb dbContext = new SQLiteDb(AppSettings.Current.SQLiteConnectionString);                               //: DbContext, IDataSource

            //Вычисляем поле SearchTerms (Table("Orders"))
            IEnumerable<Order> orders = dbContext.Orders.AsEnumerable().Select(o => { o.SearchTerms = o.BuildSearchTerms(); return o; });
            foreach (Order order in orders)
            {
                // Указать, что запись изменилась
                dbContext.Entry(order).State = EntityState.Modified;
            }
            // Сохранить изменения
            dbContext.SaveChanges();

            //Вычисляем поле SearchTerms (Table("Customers"))
            IEnumerable<Customer> customers = dbContext.Customers.AsEnumerable().Select(c => { c.SearchTerms = c.BuildSearchTerms(); return c; });
            foreach (Customer customer in customers)
            {
                // Указать, что запись изменилась
                dbContext.Entry(customer).State = EntityState.Modified;
            }
            // Сохранить изменения
            dbContext.SaveChanges();

            //Вычисляем поле SearchTerms (Table("products"))
            IEnumerable<Dish> products = dbContext.Dishes.AsEnumerable().Select(p => { p.SearchTerms = p.BuildSearchTerms(); return p; });
            foreach (Dish product in products)
            {
                // Указать, что запись изменилась
                dbContext.Entry(product).State = EntityState.Modified;
            }
            // Сохранить изменения
            dbContext.SaveChanges();
        }

        private void RecalcStreet_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDb dbContext = new SQLiteDb(AppSettings.Current.SQLiteConnectionString);                               //: DbContext, IDataSource
            IEnumerable<Order> orders = dbContext.Orders;
            IEnumerable<Street> streets = dbContext.Streets;
            foreach (Order order in orders)
            {
                /*if (order.CityId > 0)
                    continue;*/
                //if (order.RestaurantCountryCode=="KYIV")
                //{
                //    order.DeliveredCityId = 1;                          //Киев
                //}
                //else
                //{
                //    order.DeliveredCityId = 2;                          //Вишневое
                //}
                //Street OrdStreet = streets.Where(s => (s.Name == order.RestaurantAddress)&&(s.CityId == order.DeliveredCityId)).FirstOrDefault();
                //if (OrdStreet==null)
                //{
                //    OrdStreet = new Street();
                //    OrdStreet.Id = streets.Count()+1;
                //    OrdStreet.DateCreate = DateTime.UtcNow;
                //    //OrdStreet.Name = order.RestaurantAddress;
                //    OrdStreet.CityId = (long)order.DeliveredCityId;     //Город
                //    OrdStreet.TypeStreetId = 1;                         //Тип - улица
                //    dbContext.Street.Add(OrdStreet);
                //    dbContext.SaveChanges();
                //}
                //order.DeliveredStreetId = OrdStreet.Id;                 //Ссылка на справочник улиц
                //dbContext.Entry(order).State = EntityState.Modified;
                //dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Города в текстовое поле клиентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecalcCustomerCity_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDb dbContext = new SQLiteDb(AppSettings.Current.SQLiteConnectionString);                               //: DbContext, IDataSource
            IEnumerable<Customer> customers = dbContext.Customers;
            foreach (Customer customer in customers)
            {
                //if (customer.CountryCode == "KYIV")
                //{
                //    customer.City = "Киев";                      //Киев
                //}
                //else
                //{
                //    customer.City = "Вишневое";                  //Вишневое
                //}

                // Указать, что запись изменилась
                dbContext.Entry(customer).State = EntityState.Modified;
                // Сохранить изменения
            }
            dbContext.SaveChanges();
        }

        private void RecalcPhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDb dbContext = new SQLiteDb(AppSettings.Current.SQLiteConnectionString);                               //: DbContext, IDataSource
            IEnumerable<Order> orders = dbContext.Orders;
            foreach (Order order in orders)
            {
                order.PhoneNumber = "(067)9187531";
                // Указать, что запись изменилась
                dbContext.Entry(order).State = EntityState.Modified;
            }
            // Сохранить изменения
            dbContext.SaveChanges();
        }

        private void CallPhoneNumber_Click(object sender, RoutedEventArgs e)
        {
            LinphoneManager.Instance.NewOutgoingCall(PhoneHumber.Text);
        }

        public PhoneCallArgs par = new PhoneCallArgs();
        private async void Phone_Click(object sender, RoutedEventArgs e)
        {
            //await NavigationService.CreateNewViewAsync<OrderDetailsViewModel>(VM.OrderDetails.CreateArgs());
            //await NavigationService.CreateNewViewAsync(typeof(AccountSettings));
            //var ret = await NavigationService.CreateNewViewAsync(typeof(PhoneCallView));
            //var ret = await NavigationService.CreateNewViewAsync(typeof(Dialer));

            par.PhoneNumber = "1234567";
            //var ret = await NavigationService.CreateNewViewPhoneAsync(typeof(PhoneCallView), par);
            await NavigationService.CreateNewViewAsync<PhoneCallViewModel>(par);
        }

        private async void CallSettings_Click(object sender, RoutedEventArgs e)
        {
            var ret = await NavigationService.CreateNewViewAsync(typeof(Dialer));
            //var ret = await NavigationService.CreateNewViewAsync<Dialer>();
        }

        private async void BtnDbToJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dbContext = new SQLServerDb(AppSettings.Current.SQLServerConnectionString))
                {
                    var dishUnits = await dbContext.DishUnits.ToListAsync();
                    dishUnits.ForEach(x =>
                    {
                        x.Dishes = null;
                    });
                    WriteEntitiesToJson(nameof(dbContext.DishUnits), dishUnits);

                    var marks = await dbContext.Marks.ToListAsync();
                    marks.ForEach(x =>
                    {
                        x.DishMarks = null;
                    });
                    WriteEntitiesToJson(nameof(dbContext.Marks), marks);

                    var folders = await dbContext.MenuFolders.ToListAsync();
                    folders.ForEach(x =>
                    {
                        x.Dishes = null;
                        x.Parent = null;
                    });
                    WriteEntitiesToJson(nameof(dbContext.MenuFolders), folders);

                    var dishes = await dbContext.Dishes.ToListAsync();
                    dishes.ForEach(x =>
                    {
                        x.DishGarnishes = null;
                        x.DishIngredients = null;
                        x.DishMarks = null;
                        x.DishRecommends = null;
                        x.DishUnit = null;
                        x.MenuFolder = null;
                        x.OrderDishes = null;
                        x.RecommendDishes = null;
                        x.TaxType = null;
                    });
                    WriteEntitiesToJson(nameof(dbContext.Dishes), dishes);

                    var garnishes = await dbContext.DishGarnishes.ToListAsync();
                    garnishes.ForEach(x =>
                    {
                        x.Dish = null;
                    });
                    WriteEntitiesToJson(nameof(dbContext.DishGarnishes), garnishes);

                    var ingredients = await dbContext.DishIngredients.ToListAsync();
                    ingredients.ForEach(x =>
                    {
                        x.Dish = null;
                    });
                    WriteEntitiesToJson(nameof(dbContext.DishIngredients), ingredients);

                    var dishMarks = await dbContext.DishMarks.ToListAsync();
                    dishMarks.ForEach(x =>
                    {
                        x.Dish = null;
                        x.Mark = null;
                    });
                    WriteEntitiesToJson(nameof(dbContext.DishMarks), dishMarks);

                    var recommends = await dbContext.DishRecommends.ToListAsync();
                    recommends.ForEach(x =>
                    {
                        x.Dish = null;
                        x.RecommendDish = null;
                    });
                    WriteEntitiesToJson(nameof(dbContext.DishRecommends), recommends);

                    edtJsonFolderPath.Text = ApplicationData.Current.LocalFolder.Path;

                    await DialogService.ShowAsync("Выгрузка данных", "Выгрузка данных успешно завершена");
                }
            }
            catch (Exception ex)
            {
                await LogService.WriteAsync(LogType.Error, "DbDataToJson", "Save data to json", ex);
                await DialogService.ShowAsync("Выгрузка данных", "Возникла ошибка при выгрузке данных, см. лог файл");
            }
        }

        private async void WriteEntitiesToJson<T>(string fileName, IList<T> entities)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(entities, Newtonsoft.Json.Formatting.Indented);

            var fullFileName = $"{fileName}.json";
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile jsonFile = await storageFolder.CreateFileAsync(fullFileName, CreationCollisionOption.GenerateUniqueName);
            await FileIO.WriteTextAsync(jsonFile, jsonString);

            entities.Clear();
        }

        private void PhoneCallReceive_Click(object sender, RoutedEventArgs e)
        {
            PhoneCallArgs param = new PhoneCallArgs();
            param.PhoneNumber = this.PhoneHumber.Text;
            NavigationService.CreateNewViewAsync<PhoneCallReceiveViewModel>(param);
        }

        private void AudioDevices_Click(object sender, RoutedEventArgs e)
        {
            ////            Привет Я создаю приложение на базе настольных компьютеров в Windows с помощью С#.
            ////Мне нужно показать список всех доступных аудио и видео устройств в двух разных комбинированны ящиках.
            ////При выборе любого устройства из поля со списком будет установлено это конкретное устройство как стандартное.
            ////Я использую WMI.

            //Код для получения списка доступных аудиоустройств: 

            //using System.Management;

            //ManagementObjectSearcher mo =
            //      new ManagementObjectSearcher("select * from Win32_SoundDevice");

            //foreach (ManagementObject soundDevice in mo.Get())
            //{
            //    String deviceId = soundDevice.GetPropertyValue("DeviceId").ToString();
            //    String name = soundDevice.GetPropertyValue("Name").ToString();
            //}

            //// Пример 2
            //int waveInDevices = WaveIn.DeviceCount;
            //for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            //{
            //    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
            //    Console.WriteLine("Device {0}: {1}, {2} channels", waveInDevice, deviceInfo.ProductName, deviceInfo.Channels);
            //}

            ////
            //ManagementObjectSearcher mo =
            //      new ManagementObjectSearcher("select * from Win32_SoundDevice");

            //foreach (ManagementObject soundDevice in mo.Get())
            //{
            //    String deviceId = soundDevice.GetPropertyValue("DeviceId").ToString();
            //    String name = soundDevice.GetPropertyValue("Name").ToString();

            //    //saving the name  and device id in array
            //}

            ////So, one can set the device as:

            //LyncClient client = LyncClient.GetClient();
            //DeviceManager dm = client.DeviceManager;

            //dm.ActiveAudioDevice = (AudioDevice)dm.AudioDevices[0]; //or any other found after foreach
            //dm.ActiveVideoDevice = (VideoDevice)dm.VideoDevices[0]; //or any other found after foreach


            //ПВ1
            List<string> _sounddevases = new List<string>();
            int nbSound = 0;
            foreach (String device in LinphoneManager.Instance.Core.SoundDevices)
            {
                _sounddevases.Add(device);
                nbSound++;
            }
            var rdev = LinphoneManager.Instance.Core.RingerDevice;
            var sdev = LinphoneManager.Instance.Core.CaptureDevice;

            var ttt1 = Windows.Media.Render.AudioRenderCategory.Media;

            //ПВ2
            //outputDevicesListBox.Items.Clear();
            //var outputDevices = DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());
            //outputDevicesListBox.Items.Add("-- Pick output device --");
            //foreach (var device in outputDevices)
            //{
            //    //outputDevicesListBox.Items.Add(device.Name);
            //}

            //ПВ3

            ReadDevice();
            string _FolderFileName = ApplicationData.Current.LocalFolder.Path+"\\"+"VOIP"+"\\"+ "Assest\\rings\\house_keeping.mkv";
            var _ring = LinphoneManager.Instance.Core.Ring;
            LinphoneManager.Instance.Core.Ring= _FolderFileName;

        }

        private async void ReadDevice()
        {
            DeviceInformationCollection DynamicDevices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());
            DeviceInformationCollection MicrophoneDevices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioCaptureSelector());
        }

    }
}
