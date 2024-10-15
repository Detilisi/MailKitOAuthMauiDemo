using MailKitOAuthMauiDemo.ViewModels;

namespace MailKitOAuthMauiDemo.Pages;

public partial class EmailListPage : ContentPage
{

    private EmailListViewModel _viewModel;
    public EmailListPage(EmailListViewModel viewModel)
	{
		InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
        
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadEmailsCommand.Execute(null);
    }
}