using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;

namespace SignIn.Contracts;

public record SingInResponse(string IdToken, bool Success);

public interface ISignInRepository
{
    public Task<SingInResponse> Authenticate(string username, string password);
}