using MailKitOAuthMauiDemo.ViewModels;

namespace MailKitOAuthMauiDemo.Pages;

public partial class EmailListPage : ContentPage
{
	public EmailListPage(EmailListViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}