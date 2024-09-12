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
        var response = await client.AdminCreateUserAsync(new AdminCreateUserRequest 
        {
            MessageAction = "SUPPRESS",
            TemporaryPassword = request.password,
            UserAttributes = new List<AttributeType> {
                new AttributeType {
                    Name = "email",
                    Value = request.email
                },
                new AttributeType {
                    Name = "custom:CPF",
                    Value = request.username
                }
            },
            UserPoolId = "us-east-1_DBk6tjf8T",
            Username = request.username
        });

        await client.AdminSetUserPasswordAsync(new AdminSetUserPasswordRequest()
        {
            UserPoolId = "us-east-1_DBk6tjf8T",
            Username = request.username,
            Password = request.password,
            Permanent = true
        });
            
        var user = response.User;

        return new SingUpResponse(user.Username, true);
    }
}