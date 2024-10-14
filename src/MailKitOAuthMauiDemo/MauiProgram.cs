using MailKitOAuthMauiDemo.Pages;
using MailKitOAuthMauiDemo.Services;
using MailKitOAuthMauiDemo.ViewModels;
using Microsoft.Extensions.Logging;

namespace MailKitOAuthMauiDemo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            //Register servives
            builder.Services.AddSingleton<MailKitClientService>();

            //Register view models
            builder.Services.AddSingleton<OAuthViewModel>();
            builder.Services.AddSingleton<EmailListViewModel>();

            //Register Pages
            builder.Services.AddSingleton<OAuthPage>();
            builder.Services.AddSingleton<EmailListPage>();

            return builder.Build();
        }
    }
}
