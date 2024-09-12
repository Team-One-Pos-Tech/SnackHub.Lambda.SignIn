using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;

namespace SignIn.Contracts;

public record SingUpResponse(string username, bool Success);
public record SignUpRequest(string username, string password, string email);

public interface ISignUpRepository
{
    public Task<SingUpResponse> Register(SignUpRequest request);
}