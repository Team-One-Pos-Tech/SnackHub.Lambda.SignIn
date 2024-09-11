using Amazon.CognitoIdentityProvider.Model;

namespace SignIn.Contracts;

public record SingInResponse(string IdToken, bool Success);

public interface ISignInRepository
{
    public SingInResponse Authenticate(string username, string password);
}