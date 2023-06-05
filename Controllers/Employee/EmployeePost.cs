using AppRequest.Repository.Data;
using Microsoft.AspNetCore.Identity;
using AppRequest.Services.Products;
using System.Security.Claims;

namespace AppRequest.Controllers.Employee;

public class EmployeePost{
     public static string Template => "/employees";
     public static string[] Methods => new String[] { HttpMethod.Post.ToString()};
     public static Delegate Handle => Action;
     

    public static IResult Action(EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager){
        var user = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email};
        var result = userManager.CreateAsync(user,employeeRequest.Password).Result;

        if(!result.Succeeded){
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
        }

        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.Name),
            
        };

        var claimResult = userManager.AddClaimsAsync(user, userClaims).Result;
       
        if(!claimResult.Succeeded){
        return Results.BadRequest(result.Errors.First());
        }

        return Results.Created($"/EmployeePost/{user.Id}",user);
    }

}