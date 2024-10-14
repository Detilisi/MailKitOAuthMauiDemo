using CommunityToolkit.Mvvm.Input;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels.Base;
using MimeKit;
using System.Collections.ObjectModel;

namespace MailKitOAuthMauiDemo.ViewModels;

public partial class EmailListViewModel(MailKitClientService mailKitClient) : BaseViewModel(mailKitClient)
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
            if(!_mailKitClientService.ClientConnected) return;

            var mimeMessages = await _mailKitClientService.LoadMimeMessages();

            Emails.Clear();
            foreach (var message in mimeMessages)
            {
                Emails.Add(message);
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
