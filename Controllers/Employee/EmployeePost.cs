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
            return Results.BadRequest(result.Errors.First());
        }

        var claimResult = userManager.AddClaimAsync(user, new Claim("Employeecode", employeeRequest.EmployeeCode)).Result;
        if(!claimResult.Succeeded){
            return Results.BadRequest(result.Errors.First());
        }
        claimResult = userManager.AddClaimAsync(user, new Claim("Name", employeeRequest.Name)).Result;
        
           if(!claimResult.Succeeded){
            return Results.BadRequest(result.Errors.First());
        }
        return Results.Created($"/categories/{user.Id}",user);
    }

}