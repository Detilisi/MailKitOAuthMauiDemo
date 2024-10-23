using CommunityToolkit.Mvvm.ComponentModel;
using Google.Apis.Auth.OAuth2;

namespace MailKitOAuthMauiDemo.ViewModels.Base
{
    public class BaseViewModel : ObservableObject
    {
        //Properties
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        // Load ClientSecrets from SecureStorage
        protected async Task<ClientSecrets> LoadClientSecretsAsync()
        {
            try
            {
                var clientId = await SecureStorage.GetAsync("ClientId");
                var clientSecret = await SecureStorage.GetAsync("ClientSecret");

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                {
                    return null;
                }

                return new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                };
            }
            catch (Exception ex)
            {
                // Handle possible SecureStorage exceptions (like keychain access issues on iOS)
                Console.WriteLine($"Error accessing secure storage: {ex.Message}");
                return null;
            }
        }
    }
}
