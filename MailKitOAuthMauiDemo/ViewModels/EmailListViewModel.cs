using CommunityToolkit.Mvvm.Input;
using MailKit;
using MailKit.Net.Imap;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels.Base;
using MimeKit;
using System.Collections.ObjectModel;

namespace MailKitOAuthMauiDemo.ViewModels;

internal partial class EmailListViewModel(MailKitClientService mailKitClient) : BaseViewModel(mailKitClient)
{
    //Properties
    public ObservableCollection<MimeMessage> Emails { get; set; } = [];

    //Commands
    [RelayCommand]
    public async Task LoadEmails()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            using (var client = new ImapClient())
            {
                await client.ConnectAsync("imap.gmail.com", 993, true);
                await client.AuthenticateAsync("your-email@gmail.com", "your-app-password");

                // Open the INBOX
                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                // Retrieve the last 10 messages
                var recentMessages = inbox.Recent > 10 ? inbox.Recent - 10 : 0;
                var messages = inbox.Fetch(recentMessages, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId)
                                  .Select(summary => inbox.GetMessage(summary.UniqueId))
                                  .Take(10);

                Emails.Clear();
                foreach (var message in messages)
                {
                    Emails.Add(message);
                }

                await client.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error fetching emails: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

}
