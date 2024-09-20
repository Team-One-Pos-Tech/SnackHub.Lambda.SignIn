using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using SignIn.Contracts;
using SignIn.Repositories;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SignIn
{
    public record SignInBodyRequest(string Username, string Password, string Email);

    public class Function
    {
        private readonly ISignInService _signInService;

        public Function()
        {
            _signInService = new CognitoSignInService();
        }

        public Function(ISignInService signInService, ISignUpRepository singUpRepository)
        {
            _signInService = signInService;
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent,
            ILambdaContext context)
        {
            var requestBody = JsonSerializer.Deserialize<SignInBodyRequest>(apigProxyEvent.Body);

            var signInResponse = await _signInService.Authenticate(
                requestBody.Username,
                requestBody.Password);
            
            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(signInResponse),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}