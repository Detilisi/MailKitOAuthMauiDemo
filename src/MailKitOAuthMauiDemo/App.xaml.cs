namespace MailKitOAuthMauiDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            // Check if the client secrets are already stored
            var clientId = await SecureStorage.GetAsync("ClientId");
            if (string.IsNullOrEmpty(clientId))
            {
                // Secrets are not stored, store them now
                await StoreClientSecretsAsync();
            }
        }

        private async Task StoreClientSecretsAsync()
        {
            try
            {
                await SecureStorage.SetAsync("ClientId", "YOUR_CLIENT_ID.apps.googleusercontent.com");
                await SecureStorage.SetAsync("ClientSecret", "YOUR_CLIENT_SECRET");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing secrets: {ex.Message}");
            }
        }
    }
}
