using Google.Apis.Auth.OAuth2;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System.Diagnostics;

namespace MailKitOAuthMauiDemo.Services;

public class EmailService
{
    //Email operations
    public static async Task<bool> SendEmailAsync
    (
        UserCredential credential, 
        MimeMessage message, 
        CancellationToken token = default
    )
    {
        const int smtpPort = 993;
        const string smtpServer = "smpt.gmail.com";

        try
        {
            using var smtpClient = new SmtpClient();

            // Connect to Gmail SMTP server
            await smtpClient.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.SslOnConnect, token);

            // Authenticate using OAuth2 (bearer token)
            var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);
            await smtpClient.AuthenticateAsync(oauth2, token);

            // Send the email
            await smtpClient.SendAsync(message, token);

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async static Task<List<MimeMessage>> FetchEmailsAsync
    (
        UserCredential credential,
        int pageSize = 10, int skip = 0,
        CancellationToken token = default
    )
    {
        const int imapPort = 993;
        const string imapServer = "imap.gmail.com";

        var emailList = new List<MimeMessage>();
        
        try
        {
            using var imapClient = new ImapClient();

            // Connect to the Gmail IMAP server using SSL
            await imapClient.ConnectAsync(imapServer, imapPort, SecureSocketOptions.SslOnConnect, token);

            // Authenticate using OAuth2 (bearer token)
            var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);
            await imapClient.AuthenticateAsync(oauth2, token);

            // Open the inbox folder
            var inbox = imapClient.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly, token);

            // Fetch emails
            var uids = await imapClient.Inbox.SearchAsync(SearchQuery.All, token);
            for (int i = uids.Count - 1 - skip; i >= Math.Max(0, uids.Count - skip - pageSize); i--)
            {
                var uid = uids[i];
                var message = await inbox.GetMessageAsync(uid, token);
                emailList.Add(message);
            }

            return emailList;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return emailList;
        }
    }
}
