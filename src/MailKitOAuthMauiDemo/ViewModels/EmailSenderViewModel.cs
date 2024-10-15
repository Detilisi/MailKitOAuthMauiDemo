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
    private string recipient = string.Empty;

    [ObservableProperty]
    private string subject = string.Empty;

    [ObservableProperty]
    private string body = string.Empty;

    //Commands
    [RelayCommand]
    public async Task SendEmailAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            if (!_mailKitClientService.SmtpClientConnected)
            {
                await Shell.Current.DisplayAlert("Error", "Please connect your email client first.", "OK");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", "youremail@example.com"));
            message.To.Add(new MailboxAddress("", Recipient));
            message.Subject = Subject;

            message.Body = new TextPart("plain")
            {
                Text = Body
            };

            await _mailKitClientService.SendEmailAsync(message);
            
            await Shell.Current.DisplayAlert("Success", "Email sent successfully.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to send email: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
