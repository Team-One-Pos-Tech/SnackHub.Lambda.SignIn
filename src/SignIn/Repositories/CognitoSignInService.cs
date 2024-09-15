using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using SignIn.Contracts;

namespace SignIn.Repositories;

public class CognitoSignInService : ISignInService
{
        
    private readonly AmazonCognitoIdentityProviderClient _cognitoClient = new AmazonCognitoIdentityProviderClient();
    
    public async Task<SingInResponse> Authenticate(string username, string password)
    {
        var userPoolId = Environment.GetEnvironmentVariable("USER_POOL_ID");
        var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        
        var authRequest = new AdminInitiateAuthRequest()
        {
            UserPoolId = userPoolId,
            ClientId = clientId,
            AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", username },
                { "PASSWORD", password }
            }
        };
        
        AdminInitiateAuthResponse authResponse;
        
        try
        {
            authResponse = await _cognitoClient.AdminInitiateAuthAsync(authRequest);
        }
        catch (Exception e)
        {
            return new SingInResponse(null, false);
        }
        
        return new SingInResponse(authResponse.AuthenticationResult.IdToken, true);
    }
}