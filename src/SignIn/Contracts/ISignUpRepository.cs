using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;

namespace SignIn.Contracts;

public record SingUpResponse(string Username, bool Success);

public record SignUpRequest(string Username, string Password, string Email);

public interface ISignUpRepository
{
    public Task<SingUpResponse> Register(SignUpRequest request);
}