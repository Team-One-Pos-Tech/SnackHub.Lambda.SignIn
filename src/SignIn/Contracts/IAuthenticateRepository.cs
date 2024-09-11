using Amazon.CognitoIdentityProvider.Model;

namespace SignIn.Contracts;

public interface IAuthenticateRepository
{
    public AdminInitiateAuthResponse Authenticate(string username, string password);
}