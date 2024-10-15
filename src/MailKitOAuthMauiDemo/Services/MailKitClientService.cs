using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MailKitOAuthMauiDemo.Services;

public class MailKitClientService
{
    //Fields
    private readonly ImapClient _imapClient;
    private readonly SmtpClient _smptClient;
    private UserCredential? _userCredential;

    //Construction
    public MailKitClientService()
    {
        _imapClient = new ImapClient();
        _smptClient = new SmtpClient();
    }

    //Properties
    public bool ImapClientConnected => _imapClient.IsConnected && _imapClient.IsAuthenticated;
    public bool SmtpClientConnected => _smptClient.IsConnected && _smptClient.IsAuthenticated;

    //Authenitication
    public async Task<bool> AuthenticateAsync(ClientSecrets clientSecrets, string userId, CancellationToken token = default)
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
        _userCredential = await authCode.AuthorizeAsync(userId, token);

        // Refresh the token if needed
        if (_userCredential.Token.IsStale)
        {
            await _userCredential.RefreshTokenAsync(token);
        }

        return true; 
    }

    public async Task<bool> ConnectImapClientAsync(CancellationToken token = default)
    {
        if(_userCredential == null) return false;
        
        const int imapPort = 993;
        const string imapServer = "imap.gmail.com";
        var oauth2 = new SaslMechanismOAuth2(_userCredential.UserId, _userCredential.Token.AccessToken);

        // Connect and authenticate
        await _imapClient.ConnectAsync(imapServer, imapPort, SecureSocketOptions.SslOnConnect, token);
        await _imapClient.AuthenticateAsync(oauth2, token);

        return true; 
    }

    public async Task<bool> ConnectSmptClientAsync(CancellationToken token = default)
    {
        if (_userCredential == null) return false;
        const int smtpPort = 465;
        const string smtpServer = "smtp.gmail.com";
        var oauth2 = new SaslMechanismOAuth2(_userCredential.UserId, _userCredential.Token.AccessToken);

        // Connect and authenticate
        await _smptClient.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.SslOnConnect, token);
        await _smptClient.AuthenticateAsync(oauth2, token);

        return true;
    }

    //Email operations
    public async Task<bool> SendEmailAsync(MimeMessage message, CancellationToken token = default)
    {
        var result = await _smptClient.SendAsync(message, token);
        return true;
    }

    public async Task<List<MimeMessage>> LoadEmailMessagesAsync(CancellationToken token = default)
    {
        var emailList = new List<MimeMessage>();
        if (!ImapClientConnected) return emailList;

        var inbox = _imapClient.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadOnly, token);

        // Retrieve the last 10 messages
        var recentMessages = inbox.Recent > 10 ? inbox.Recent - 10 : 0;
        var messages = inbox.Fetch(recentMessages, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId, token)
                          .Select(summary => inbox.GetMessage(summary.UniqueId))
                          .Take(10);

        foreach (var message in messages)
        {
            emailList.Add(message);
        }

        return emailList;
    }

    public async Task DisconnectImapClientAsync()
    {
        if (_imapClient.IsConnected)
        {
            await _imapClient.DisconnectAsync(true);
        }
    }

    public async Task DisconnectSmtpClientAsync()
    {
        if (_smptClient.IsConnected)
        {
            await _smptClient.DisconnectAsync(true);
        }
    }
}
