using CommunityToolkit.Mvvm.ComponentModel;

namespace MailKitOAuthMauiDemo.ViewModels.Base
{
    internal class BaseViewModel : ObservableObject
    {
        // Loading state to bind to the UI
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
    }
}
