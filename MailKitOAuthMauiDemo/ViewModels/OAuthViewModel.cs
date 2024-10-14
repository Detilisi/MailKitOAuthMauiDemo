using CommunityToolkit.Mvvm.Input;
using Google.Apis.Auth.OAuth2;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels.Base;

namespace MailKitOAuthMauiDemo.ViewModels;

internal partial class OAuthViewModel(MailKitClientService mailKitClient) : BaseViewModel(mailKitClient)
{
    //Fields
    private const string GMailAccount = "username@gmail.com";

    //Commands
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

            var authenicationSuccessful = await _mailKitClientService.AuthenticateAsync(clientSecrets, GMailAccount);
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
