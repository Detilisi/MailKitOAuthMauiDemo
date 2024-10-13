using CommunityToolkit.Mvvm.Input;
using Google.Apis.Auth.OAuth2;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels.Base;

namespace MailKitOAuthMauiDemo.ViewModels;

internal partial class OAuthViewModel : BaseViewModel
{
    private MailKitClientService _mailKitClient;
    private const string GMailAccount = "username@gmail.com";

    public OAuthViewModel()
    {
        _mailKitClient = new MailKitClientService();
    }

    [RelayCommand]
    public async Task ConnectMailKitAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var clientSecrets = new ClientSecrets
            {
                ClientId = "XXX.apps.googleusercontent.com",
                ClientSecret = "XXX"
            };

            var authenicationSuccessful = await _mailKitClient.AuthenticateAsync(clientSecrets, GMailAccount);
            if (authenicationSuccessful)
            {
                IsBusy = false;

                //Open email list page
            }
            else 
            {
                //Try again
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
