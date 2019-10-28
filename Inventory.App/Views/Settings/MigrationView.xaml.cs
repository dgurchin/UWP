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

using Inventory.Common;
using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Inventory.Views
{
    public partial class MigrationView : ContentDialog
    {
        private string _connectionString = null;
        private bool _isOnlyCopyProductImages;

        public MigrationView(string connectionString, bool isOnlyCopyProductImages = false)
        {
            _connectionString = connectionString;
            _isOnlyCopyProductImages = isOnlyCopyProductImages;
            ViewModel = ServiceLocator.Current.GetService<MigrateViewModel>();
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public MigrateViewModel ViewModel { get; }

        public Result Result { get; private set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_isOnlyCopyProductImages)
            {
                await ViewModel.CopyProductImagesAsync(_connectionString);
            }
            else
            {
                await ViewModel.ExecuteAsync(_connectionString);
            }
            
            Result = ViewModel.Result;
        }

        private void OnOkClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void OnCancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = Result.Ok("Operation cancelled by user");
        }
    }
}
