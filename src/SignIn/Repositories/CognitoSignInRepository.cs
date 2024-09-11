using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using SignIn.Contracts;

namespace SignIn.Repositories;

public class CognitoSignInRepository : ISignInRepository
{
        
    private readonly AmazonCognitoIdentityProviderClient _cognitoClient = new AmazonCognitoIdentityProviderClient();
    
    public async Task<SingInResponse> Authenticate(string username, string password)
    {
        var authRequest = new AdminInitiateAuthRequest()
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