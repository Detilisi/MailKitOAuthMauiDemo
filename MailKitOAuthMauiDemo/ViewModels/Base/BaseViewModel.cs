using CommunityToolkit.Mvvm.ComponentModel;
using MailKitOAuthMauiDemo.Services;

namespace MailKitOAuthMauiDemo.ViewModels.Base
{
    internal class BaseViewModel(MailKitClientService mailKitClient) : ObservableObject
    {
        //Services
        protected MailKitClientService _mailKitClientService = mailKitClient;

        //Properties
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
    }
}
