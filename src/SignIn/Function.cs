using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using SignIn.Contracts;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SignIn
{

    public class Function
    {
        private const string DefaultPassword = "Default-password-99!";
        
        private static readonly HttpClient client = new HttpClient();
        
        private static readonly AmazonCognitoIdentityProviderClient cognitoClient = new AmazonCognitoIdentityProviderClient();
        
        private readonly ISignInRepository _signInRepository;
        
        private readonly ISignUpRepository _singUpRepository;

        public Function(ISignInRepository signInRepository, ISignUpRepository singUpRepository)
        {
            _signInRepository = signInRepository;
            _singUpRepository = singUpRepository;
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var requestBody = JsonSerializer.Deserialize<Dictionary<string, string>>(apigProxyEvent.Body);
            var username = requestBody["Username"];
            var password = requestBody["Password"];
            if(!requestBody.TryGetValue("Email", out var email))
            {
                email = "";
            }

            var authResponse = await _signInRepository.Authenticate(username, password);

            if (authResponse.Success)
            {
                return CreateResponse(authResponse);
            }
            
            var registerRequest = new RegisterRequest(username, password, email);
                
            var registerResponse = _singUpRepository.Register(registerRequest);
                
            authResponse = await _signInRepository.Authenticate(username, password);

            // var authRequest = new AdminInitiateAuthRequest
            // {
            //     UserPoolId = "us-east-1_DBk6tjf8T", // Replace with your user pool ID
            //     ClientId = "4g9i9qigcm7mq82s2r7v939uae", // Replace with your client ID
            //     AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
            //     AuthParameters = new Dictionary<string, string>
            //     {
            //         { "USERNAME", username },
            //         { "PASSWORD", password }
            //     }
            // };
            //
            // var authResponse = await cognitoClient.AdminInitiateAuthAsync(authRequest);

            return CreateResponse(authResponse);
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
