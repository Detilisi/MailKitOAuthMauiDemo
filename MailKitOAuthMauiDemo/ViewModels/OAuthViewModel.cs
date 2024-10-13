using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit.Net.Imap;
using MailKit.Security;

namespace MailKitOAuthMauiDemo.ViewModels;

internal partial class OAuthViewModel : ObservableObject
{
    private const int ImapPort = 993;
    private const string ImapServer = "imap.gmail.com";
    private const string GMailAccount = "username@gmail.com";

    // Client secrets (Store securely)
    private readonly string _clientId = "XXX.apps.googleusercontent.com";
    private readonly string _clientSecret = "XXX";

  
    // Loading state to bind to the UI (optional)
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private bool CanExecuteConnect => !IsBusy;
    
    [RelayCommand]
    private async Task ConnectMailKitAsync()
    {
        try
        {
            IsBusy = true;

            var clientSecrets = new ClientSecrets
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            };

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { "https://mail.google.com/" },
                ClientSecrets = clientSecrets,
                LoginHint = GMailAccount
            });

            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            var credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

            // Refresh the token if needed
            if (credential.Token.IsStale)
            {
                await credential.RefreshTokenAsync(CancellationToken.None);
            }

            var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

            // Connect to the IMAP server
            using (var client = new ImapClient())
            {
                await client.ConnectAsync(ImapServer, ImapPort, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(oauth2);
                await client.DisconnectAsync(true);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
