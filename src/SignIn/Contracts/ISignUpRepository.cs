using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;

namespace SignIn.Contracts;

public record SingUpResponse(string username, bool Success);
public record RegisterRequest(string username, string password, string email);

public interface ISignUpRepository
{
    public Task<SingUpResponse> Register(RegisterRequest request);
}