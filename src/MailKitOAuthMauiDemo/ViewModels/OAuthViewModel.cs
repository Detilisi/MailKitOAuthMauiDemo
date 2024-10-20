﻿using CommunityToolkit.Mvvm.Input;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels.Base;

namespace MailKitOAuthMauiDemo.ViewModels;

public partial class OAuthViewModel(MailKitClientService mailKitClient) : BaseViewModel(mailKitClient)
{
    //Fields
    private string UserEmailAddress = "tester@gmail.com";
    
    //Commands
    [RelayCommand]
    public async Task ConnectMailKitAsync()
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

            // Perform authentication
            bool isAuthenticated = await _mailKitClientService.AuthenticateAsync(clientSecrets, UserEmailAddress);

            if (isAuthenticated)
            {
                // Navigate to EmailListPage upon successful authentication
                await Shell.Current.GoToAsync("//EmailListPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Authentication Failed", "Please check your credentials and try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            await Shell.Current.DisplayAlert("Error", $"An error occurred during authentication: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
