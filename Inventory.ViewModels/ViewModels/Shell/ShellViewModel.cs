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
using System.Threading.Tasks;

using Inventory.Common;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class ShellArgs
    {
        public Type ViewModel { get; set; }
        public object Parameter { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class ShellViewModel : ViewModelBase
    {
        private UserInfo _userInfo;

        public ShellViewModel(ILoginService loginService, ICommonServices commonServices) : base(commonServices)
        {
            IsLocked = !loginService.IsAuthenticated;
        }

        private bool _isLocked = false;
        public bool IsLocked
        {
            get => _isLocked;
            set => Set(ref _isLocked, value);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }

        private string _message = Messages.StatusMessageReady;
        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        private bool _isError = false;
        public bool IsError
        {
            get => _isError;
            set => Set(ref _isError, value);
        }

        public UserInfo UserInfo
        {
            get => _userInfo;
            protected set => Set(ref _userInfo, value);
        }

        public ShellArgs ViewModelArgs { get; protected set; }

        virtual public Task LoadAsync(ShellArgs args)
        {
            ViewModelArgs = args;
            if (ViewModelArgs != null)
            {
                UserInfo = ViewModelArgs.UserInfo;
                NavigationService.Navigate(ViewModelArgs.ViewModel, ViewModelArgs.Parameter);
            }
            return Task.CompletedTask;
        }
        virtual public void Unload()
        {
        }

        virtual public void Subscribe()
        {
            MessageService.Subscribe<ILoginService, bool>(this, OnLoginMessage);
            MessageService.Subscribe<ViewModelBase, string>(this, OnMessage);
        }

        virtual public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        private async void OnLoginMessage(ILoginService loginService, string message, bool isAuthenticated)
        {
            if (message == Messages.AuthenticationChanged)
            {
                await ContextService.RunAsync(() =>
                {
                    IsLocked = !isAuthenticated;
                });
            }
        }

        private async void OnMessage(ViewModelBase viewModel, string message, string status)
        {
            switch (message)
            {
                case Messages.StatusMessage:
                case Messages.StatusError:
                    if (viewModel.ContextService.ContextID == ContextService.ContextID)
                    {
                        IsError = message == Messages.StatusError;
                        SetStatus(status);
                    }
                    break;

                case Messages.EnableThisView:
                case Messages.DisableThisView:
                    if (viewModel.ContextService.ContextID == ContextService.ContextID)
                    {
                        IsEnabled = message == Messages.EnableThisView;
                        SetStatus(status);
                    }
                    break;

                case Messages.EnableOtherViews:
                case Messages.DisableOtherViews:
                    if (viewModel.ContextService.ContextID != ContextService.ContextID)
                    {
                        await ContextService.RunAsync(() =>
                        {
                            IsEnabled = message == Messages.EnableOtherViews;
                            SetStatus(status);
                        });
                    }
                    break;

                case Messages.EnableAllViews:
                case Messages.DisableAllViews:
                    await ContextService.RunAsync(() =>
                    {
                        IsEnabled = message == Messages.EnableAllViews;
                        SetStatus(status);
                    });
                    break;
            }
        }

        private void SetStatus(string message)
        {
            message = message ?? "";
            message = message.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
            Message = message;
        }
    }
}
