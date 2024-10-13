using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit.Net.Imap;
using MailKit.Security;

namespace MailKitOAuthMauiDemo.Services;

internal class MailKitClientService
{
    private ImapClient _client;

    private const int ImapPort = 993;
    private const string ImapServer = "imap.gmail.com";
    
    public MailKitClientService()
    {
        _client = new ImapClient();
    }

    public async Task<bool> AuthenticateAsync(ClientSecrets clientSecrets, string userId, CancellationToken cancellationToken)
    {
        var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            DataStore = new FileDataStore("CredentialCacheFolder", false),
            Scopes = new[] { "https://mail.google.com/" },
            ClientSecrets = clientSecrets,
            LoginHint = userId
        });

        var codeReceiver = new LocalServerCodeReceiver();
        var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);
        var credential = await authCode.AuthorizeAsync(userId, cancellationToken);

        // Refresh the token if needed
        if (credential.Token.IsStale)
        {
            await credential.RefreshTokenAsync(cancellationToken);
        }

        var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

        // Connect and authenticate
        await _client.ConnectAsync(ImapServer, ImapPort, SecureSocketOptions.SslOnConnect);
        await _client.AuthenticateAsync(oauth2);

        return true; // Indicate success
    }

    public ImapClient GetClient() => _client;
    
    public async Task DisconnectAsync()
    {
        if (_client.IsConnected)
        {
            await _client.DisconnectAsync(true);
        }
    }
}
