using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloWorld
{

    public class Function
    {
        private const string DefaultPassword = "Default-password-99!";
        
        private static readonly HttpClient client = new HttpClient();
        
        private static readonly AmazonCognitoIdentityProviderClient cognitoClient = new AmazonCognitoIdentityProviderClient();

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            var requestBody = JsonSerializer.Deserialize<Dictionary<string, string>>(apigProxyEvent.Body);
            var username = requestBody["Username"];
            var password = requestBody["Password"];

            var authRequest = new AdminInitiateAuthRequest
            {
                UserPoolId = "us-east-1_DBk6tjf8T", // Replace with your user pool ID
                ClientId = "4g9i9qigcm7mq82s2r7v939uae", // Replace with your client ID
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "PASSWORD", password }
                }
            };
            
            var authResponse = await cognitoClient.AdminInitiateAuthAsync(authRequest);

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(authResponse),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}
