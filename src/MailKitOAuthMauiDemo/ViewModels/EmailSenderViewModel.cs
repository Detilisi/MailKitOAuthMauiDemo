using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels.Base;
using MimeKit;

namespace MailKitOAuthMauiDemo.ViewModels;

public partial class EmailSenderViewModel(MailKitClientService mailKitClient) : BaseViewModel(mailKitClient)
{
    //Properties
    [ObservableProperty]
    private string sender;

    [ObservableProperty]
    private string recipient = string.Empty;

    [ObservableProperty]
    private string subject = string.Empty;

    [ObservableProperty]
    private string body = string.Empty;

    //Commands
    [RelayCommand]
    public async Task CancelAsync()
    {
        if (IsBusy) return;
        await Shell.Current.GoToAsync("//EmailListPage");
    }

    [RelayCommand]
    public async Task SendEmailAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var clientSecrets = await LoadClientSecretsAsync();
            if (clientSecrets == null)
            {
                await Shell.Current.DisplayAlert("Error", "Client secrets not found in secure storage.", "OK");
                return;
            }
            var userCredential = await GoogleOAuthService.GetGoogleUserCredentialAsync(clientSecrets);

            // Compose metheod
            var message = new MimeMessage()
            {
                Subject = Subject,
                Body = new TextPart("plain"){ Text = Body },
            };
            message.From.Add(new MailboxAddress(userCredential.UserId, userCredential.UserId));
            message.To.Add(new MailboxAddress("", Recipient));

            await EmailService.SendEmailAsync(userCredential, message);
            await Shell.Current.DisplayAlert("Success", "Email sent successfully.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to send email: {ex.Message}", "OK");
        }
        finally
        {
            await _mailKitClientService.DisconnectSmtpClientAsync();
            IsBusy = false;
        }
    }
}
