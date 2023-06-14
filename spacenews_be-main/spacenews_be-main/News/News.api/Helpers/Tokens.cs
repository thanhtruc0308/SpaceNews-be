using System.Security.Claims;
using News.api.Auth;
using News.api.Model;
using Newtonsoft.Json;
using News.api.Data;
using Microsoft.AspNetCore.Mvc;

namespace News.api.Helpers;

public class Tokens
{
    public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory,string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings, string roleId)
    {
        var response = new
        {
            id = identity.Claims.Single(c => c.Type == "id").Value,
            auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
            expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
            role = roleId
        };

        return JsonConvert.SerializeObject(response, serializerSettings);
    }
}