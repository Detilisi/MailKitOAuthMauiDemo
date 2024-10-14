using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;

namespace MailKitOAuthMauiDemo.Services;

public class MailKitClientService
{
    //Constants
    private const int ImapPort = 993;
    private const string ImapServer = "imap.gmail.com";

    //Fields
    private readonly ImapClient _client;
    
    //Construction
    public MailKitClientService()
    {
        _client = new ImapClient();
    }

    //Properties
    public bool ClientConnected => _client.IsConnected && _client.IsAuthenticated;

    //Methods
    public async Task<bool> AuthenticateAsync(ClientSecrets clientSecrets, string userId, CancellationToken cancellationToken = default)
    {
        var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            DataStore = new FileDataStore("CredentialCacheFolder", false),
            Scopes = ["https://mail.google.com/"],
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
        await _client.ConnectAsync(ImapServer, ImapPort, SecureSocketOptions.SslOnConnect, cancellationToken);
        await _client.AuthenticateAsync(oauth2, cancellationToken);

        return true; // Indicate success
    }

    public async Task<List<MimeMessage>> LoadMimeMessages()
    {
        var emailList = new List<MimeMessage>();
        if (!ClientConnected) return emailList;

        var inbox = _client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadOnly);

        // Retrieve the last 10 messages
        var recentMessages = inbox.Recent > 10 ? inbox.Recent - 10 : 0;
        var messages = inbox.Fetch(recentMessages, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId)
                          .Select(summary => inbox.GetMessage(summary.UniqueId))
                          .Take(10);

        foreach (var message in messages)
        {
            emailList.Add(message);
        }

        return emailList;
    }

    public async Task DisconnectAsync()
    {
        if (_client.IsConnected)
        {
            await _client.DisconnectAsync(true);
        }
    }
}
