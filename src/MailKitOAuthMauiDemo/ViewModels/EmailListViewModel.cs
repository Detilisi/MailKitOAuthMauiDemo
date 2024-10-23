using CommunityToolkit.Mvvm.Input;
using MailKitOAuthMauiDemo.Models;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels.Base;
using System.Collections.ObjectModel;

namespace MailKitOAuthMauiDemo.ViewModels;

public partial class EmailListViewModel(MailKitClientService mailKitClient) : BaseViewModel(mailKitClient)
{
    //Properties
    public ObservableCollection<EmailModel> Emails { get; set; } = [];

    //Commands
    [RelayCommand]
    public async Task WriteEmail()
    {
        if (IsBusy) return;
        await Shell.Current.GoToAsync("//EmailSenderPage");
    }

    [RelayCommand]
    public async Task LoadEmails()
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

            // Get messages
            var userCredential = await GoogleOAuthService.GetGoogleUserCredentialAsync(clientSecrets);
            var mimeMessages = await EmailService.FetchEmailsAsync(userCredential);

            Emails.Clear();
            foreach (var message in mimeMessages)
            {
                Emails.Add(new EmailModel
                {
                    Subject = message.Subject,
                    From = message.From.ToString(),
                    Date = message.Date.ToString("g") 
                });
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fetching Emails Failed", ex.Message, "Ok");
        }
        finally
        {
            IsBusy = false;
        }
    }

}
