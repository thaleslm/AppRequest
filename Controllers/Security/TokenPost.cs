using AppRequest.Repository.Data;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AppRequest.Controllers.Security;

public class TokenPost{
     public static string Template => "/token";
     public static string[] Methods => new String[] { HttpMethod.Post.ToString()};
     public static Delegate Handle => Action;
    [AllowAnonymous]
    public static IResult Action(LoginRequest loginRequest,IConfiguration configuration, UserManager<IdentityUser> userManager){
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
        if(user == null)
            return Results.BadRequest();
        if(!userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
            return Results.BadRequest();

        var claims = userManager.GetClaimsAsync(user).Result;
        var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email,loginRequest.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)//para saber qual usuario cadastrou para ter os logs
            });
        subject.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);
        //Defini informações do Token e a descrição
        var TokenDescription = new SecurityTokenDescriptor
        {
           Subject = subject,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["Jwr:Audience"],
            Issuer = configuration["Jwr:Issuer"]
        };        

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(TokenDescription);
        return Results.Ok(new{
            token = tokenHandler.WriteToken(token)
        });

    }

}