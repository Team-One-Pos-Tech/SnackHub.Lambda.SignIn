using Moq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using SignIn.Contracts;
using Xunit;

namespace SignIn.Tests
{
  public class FunctionTest
  {
    private static readonly HttpClient client = new HttpClient();
    
    private Mock<IAuthenticateRepository> _authRepository;

    private void BeforeTestStarting()
    {
      _authRepository = new Mock<IAuthenticateRepository>();
    }
    
    [Fact]
    public async Task AuthenticateUser()
    {
      // Arrange
      BeforeTestStarting();
      
      
      var request = new APIGatewayProxyRequest();
      var context = new TestLambdaContext();

      var cpf = "53469738009";
      var password = "DefaultPassword!";
      
      request.Body = $@"
        {{
            ""Username"": ""{cpf}"",
            ""Password"": ""{password}""
        }}";

      _authRepository.Setup(repository => repository.Authenticate(cpf, password))
        .Returns(
          new AdminInitiateAuthResponse()
          {
            AuthenticationResult = new AuthenticationResultType()
            {
              IdToken = "eyJraWQiOiJrZXktdjEiLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiI0ZzlpOXFpZ2NtN21xODJzMnI3djkzOWF1ZSIsImF1ZCI6IkRiNmtyZm9lZG5k"
            }
          }
        );
            
      var function = new Function(_authRepository.Object);
      
      // Act
      var response = await function.FunctionHandler(request, context);
        
      // Assert
      response.StatusCode.Should().Be(200);
      
      var authResponse = JsonSerializer.Deserialize<AdminInitiateAuthResponse>(response.Body);
      authResponse.AuthenticationResult.IdToken.Should().NotBe(null);
      
      _authRepository.Verify(x => x.Authenticate(cpf, password), Times.Once);

    }
  }
}