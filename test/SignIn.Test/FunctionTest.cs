using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using Xunit;

namespace SignIn.Tests
{
  public class FunctionTest
  {
    private static readonly HttpClient client = new HttpClient();

    [Fact]
    public async Task AuthenticateUseByCPF()
    {
      // Arrange
      var request = new APIGatewayProxyRequest();
      var context = new TestLambdaContext();
      request.Body = @"
        {
            ""Cpf"": ""53469738009"",
        }";
            
      var function = new Function();
      
      // Act
      var response = await function.FunctionHandler(request, context);
        
      // Assert
      response.StatusCode.Should().Be(200);
      
      var authResponse = JsonSerializer.Deserialize<AdminInitiateAuthResponse>(response.Body);
      authResponse.AuthenticationResult.IdToken.Should().NotBe(null);

    }
  }
}