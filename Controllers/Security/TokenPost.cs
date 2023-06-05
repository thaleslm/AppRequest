using AppRequest.Repository.Data;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AppRequest.Controllers.Security;

public class TokenPost{
     public static string Template => "/token";
     public static string[] Methods => new String[] { HttpMethod.Post.ToString()};
     public static Delegate Handle => Action;
    
    public static IResult Action(LoginRequest loginRequest, UserManager<IdentityUser> userManager,IConfiguration configuration){
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
        if(user == null)
            return Results.BadRequest();
        if(!userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
            return Results.BadRequest();

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]));
        

        //Defini informações do Token e a descrição
        var TokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email,loginRequest.Email),
            }),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Audience = "AppRequest",
            Issuer = "Issuer"
        };
        

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(TokenDescription);
        return Results.Ok(new{
            token = tokenHandler.WriteToken(token)
        });



    }

}