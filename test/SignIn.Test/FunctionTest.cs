using System.Net;
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
    private Mock<ISignInRepository> _authRepository;

    private void BeforeTestStarting()
    {
      _authRepository = new Mock<ISignInRepository>();
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
          new SingInResponse("eyJraWQiOiJrZXktdjEiLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiI0ZzlpOXFpZ2NtN21xODJzMnI3djkzOWF1ZSIsImF1ZCI6IkRiNmtyZm9lZG5k", true)
        );
            
      var function = new Function(_authRepository.Object);
      
      // Act
      var response = await function.FunctionHandler(request, context);
        
      // Assert
      response.StatusCode.Should().Be(200);
      
      var authResponse = JsonSerializer.Deserialize<SingInResponse>(response.Body);
      authResponse.IdToken.Should().NotBe(null);
      
      _authRepository.Verify(x => x.Authenticate(cpf, password), Times.Once);

    }
  }
}