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
    public record SignUpBodyRequest(string Username, string Password, string Email);

    public class Function
    {
        private readonly ISignInService _signInService;

        private readonly ISignUpRepository _singUpRepository;

        public Function()
        {
            _signInService = new CognitoSignInService();
            _singUpRepository = new CognitoSignUpRepository();
        }

        public Function(ISignInService signInService, ISignUpRepository singUpRepository)
        {
            _signInService = signInService;
            _singUpRepository = singUpRepository;
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent,
            ILambdaContext context)
        {
            var requestBody = JsonSerializer.Deserialize<SignUpBodyRequest>(apigProxyEvent.Body);

            var signInResponse = await _signInService.Authenticate(
                requestBody.Username,
                requestBody.Password);

            if (signInResponse.Success)
            {
                return CreateResponse(signInResponse);
            }

            var signUpRequest = new SignUpRequest(
                requestBody.Username,
                requestBody.Password,
                requestBody.Email);

            await _singUpRepository.Register(signUpRequest);

            signInResponse = await _signInService.Authenticate(
                requestBody.Username, 
                requestBody.Password);

            return CreateResponse(signInResponse);
        }

        private static APIGatewayProxyResponse CreateResponse(SingInResponse authResponse)
        {
            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(authResponse),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}