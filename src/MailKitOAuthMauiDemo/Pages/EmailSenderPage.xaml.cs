using MailKitOAuthMauiDemo.ViewModels;

namespace MailKitOAuthMauiDemo.Pages;

public partial class EmailSenderPage : ContentPage
{
    public EmailSenderPage(EmailSenderViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}