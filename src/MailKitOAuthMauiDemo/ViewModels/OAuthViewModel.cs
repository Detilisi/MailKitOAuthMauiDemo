using CommunityToolkit.Mvvm.Input;
using Google.Apis.Auth.OAuth2;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels.Base;

namespace MailKitOAuthMauiDemo.ViewModels;

public partial class OAuthViewModel(MailKitClientService mailKitClient) : BaseViewModel(mailKitClient)
{
    //Commands

    [RelayCommand]
    public async Task ConnectMailKitAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            // Retrieve client secrets from secure storage
            var clientSecrets = await LoadClientSecretsAsync();

            if (clientSecrets == null)
            {
                await Shell.Current.DisplayAlert("Error", "Client secrets not found in secure storage.", "OK");
                return;
            }

            // Perform authentication
            var noUserId = " "; //trust me bro
            bool isAuthenticated = await _mailKitClientService.AuthenticateAsync(clientSecrets, noUserId);

            if (isAuthenticated)
            {
                // Navigate to EmailListPage upon successful authentication
                await Shell.Current.GoToAsync("//EmailListPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Authentication Failed", "Please check your credentials and try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            await Shell.Current.DisplayAlert("Error", $"An error occurred during authentication: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Load ClientSecrets from SecureStorage
    private async Task<ClientSecrets> LoadClientSecretsAsync()
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
