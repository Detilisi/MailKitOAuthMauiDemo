using MailKitOAuthMauiDemo.ViewModels;

namespace MailKitOAuthMauiDemo.Pages;

public partial class OAuthPage : ContentPage
{
    public OAuthPage(OAuthViewModel viewModel)
	{
		BindingContext = viewModel;

		InitializeComponent();
	}
}