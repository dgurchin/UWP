using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Inventory.Controls
{
    public sealed partial class StreetSuggestBox : UserControl
    {
        private IStreetService StreetService { get; }

        public StreetSuggestBox()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                StreetService = ServiceLocator.Current.GetService<IStreetService>();
            }
            InitializeComponent();
        }

        #region Items
        public IList<StreetModel> Items
        {
            get { return (IList<StreetModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IList<StreetModel>), typeof(StreetSuggestBox), new PropertyMetadata(null));
        #endregion

        #region DisplayText
        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(nameof(DisplayText), typeof(string), typeof(StreetSuggestBox), new PropertyMetadata(null));
        #endregion

        #region IsReadOnly*
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        private static void IsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StreetSuggestBox;
            control.suggestBox.Mode = ((bool)e.NewValue == true) ? FormEditMode.ReadOnly : FormEditMode.Auto;
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(StreetSuggestBox), new PropertyMetadata(false, IsReadOnlyChanged));
        #endregion

        #region IsRequired
        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(CustomerSuggestBox), new PropertyMetadata(null));
        #endregion

        #region CityQuery
        public int CityQuery
        {
            get { return (int)GetValue(CityQueryProperty); }
            set { SetValue(CityQueryProperty, value); }
        }

        public static readonly DependencyProperty CityQueryProperty = DependencyProperty.Register(nameof(CityQuery), typeof(int), typeof(StreetSuggestBox), new PropertyMetadata(null));
        #endregion

        #region StreetSelectedCommand
        public ICommand StreetSelectedCommand
        {
            get { return (ICommand)GetValue(StreetSelectedCommandProperty); }
            set { SetValue(StreetSelectedCommandProperty, value); }
        }

        public static readonly DependencyProperty StreetSelectedCommandProperty = DependencyProperty.Register(nameof(StreetSelectedCommand), typeof(ICommand), typeof(StreetSuggestBox), new PropertyMetadata(null));
        #endregion

        private async void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (args.CheckCurrent())
                {
                    if (string.IsNullOrEmpty(sender.Text))
                    {
                        Items = null;
                        DisplayText = null;
                        StreetSelectedCommand?.TryExecute(null);
                    }
                    else
                    {
                        Items = await GetItems(sender.Text);
                        DisplayText = sender.Text;
                    }
                }
            }
        }

        private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            StreetSelectedCommand?.TryExecute(args.SelectedItem);
            DisplayText = args.SelectedItem.ToString();
        }

        private async Task<IList<StreetModel>> GetItems(string query)
        {
            var request = new DataRequest<Street>()
            {
                Query = query,
                OrderBy = x => x.Name
            };

            if (CityQuery > 0)
            {
                request.Where = x => x.CityId == CityQuery;
            }

            return await StreetService.GetStreetsAsync(request);
        }
    }
}
