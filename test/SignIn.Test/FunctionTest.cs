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
    public async Task AuthenticateUseByCpf()
    {
      // Arrange
      var request = new APIGatewayProxyRequest();
      var context = new TestLambdaContext();

      var cpf = "53469738009";
      
      request.Body = $@"
        {{
            ""Cpf"": ""{cpf}"",
        }}";
            
      var function = new Function();
      
      // Act
      var response = await function.FunctionHandler(request, context);
        
      // Assert
      response.StatusCode.Should().Be(200);
      
      var authResponse = JsonSerializer.Deserialize<AdminInitiateAuthResponse>(response.Body);
      authResponse.AuthenticationResult.IdToken.Should().NotBe(null);
      
      _authRepository.Verify(x => x.Authenticate(cpf), Times.Once);

    }
  }
}