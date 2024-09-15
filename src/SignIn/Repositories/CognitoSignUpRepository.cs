using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using SignIn.Contracts;
using SignUpRequest = SignIn.Contracts.SignUpRequest;

namespace SignIn.Repositories;

public class CognitoSignUpRepository : ISignUpRepository
{
    public async Task<SingUpResponse> Register(SignUpRequest request)
    {
        var client = new AmazonCognitoIdentityProviderClient();
        
        var userPoolId = Environment.GetEnvironmentVariable("USER_POOL_ID");
        
        var response = await client.AdminCreateUserAsync(new AdminCreateUserRequest 
        {
            MessageAction = "SUPPRESS",
            TemporaryPassword = request.Password,
            UserAttributes = new List<AttributeType> {
                new AttributeType {
                    Name = "email",
                    Value = request.Email
                },
                new AttributeType {
                    Name = "custom:CPF",
                    Value = request.Username
                }
            },
            UserPoolId = userPoolId,
            Username = request.Username
        });

        await client.AdminSetUserPasswordAsync(new AdminSetUserPasswordRequest()
        {
            UserPoolId = userPoolId,
            Username = request.Username,
            Password = request.Password,
            Permanent = true
        });
            
        var user = response.User;

        return new SingUpResponse(user.Username, true);
    }
}