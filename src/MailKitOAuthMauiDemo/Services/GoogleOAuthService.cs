using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Diagnostics;

namespace MailKitOAuthMauiDemo.Services;

public class GoogleOAuthService
{
    public static async Task<UserCredential> GetGoogleUserCredentialAsync
    (
        ClientSecrets clientSecrets, 
        CancellationToken token = default
    )
    {
        const string anonymousUserId = "anonymous-user";
        const string cacheFolder = "CredentialCacheFolder";

        try
        {
            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                DataStore = new FileDataStore(cacheFolder, false),
                Scopes = [ PeopleServiceService.Scope.UserinfoEmail, PeopleServiceService.Scope.UserinfoProfile ] 
            });

            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);
            var userCredential = await authCode.AuthorizeAsync(anonymousUserId, token);
            
            return userCredential;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to get Google OAuth2 credentials: {ex.Message}");
            throw;
        }
    }

    public static async Task<EmailAddress> GetGoogleEmailAddressAsync
    (
        UserCredential credential, 
        CancellationToken token = default
    )
    {
        const string resourceUrl = "people/me";
        const string applicationName = "application-name";
        const string personFields = "emailAddresses, names";

        try
        {
            var service = new PeopleServiceService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            var executer = service.People.Get(resourceUrl);
            executer.PersonFields = personFields;
            var profile = await executer.ExecuteAsync(token);

            return profile.EmailAddresses.FirstOrDefault() ?? throw new Exception("Google user email not found");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to get Google user email: {ex.Message}");
            throw;
        }   
    }
}
