using AppRequest.Repository.Data;
using Microsoft.AspNetCore.Identity;
using AppRequest.Services.Products;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AppRequest.Controllers.Employee;

public class EmployeePost{
     public static string Template => "/employees";
     public static string[] Methods => new String[] { HttpMethod.Post.ToString()};
     public static Delegate Handle => Action;
     
    [Authorize(Policy = "EmployeePolicy")]//precisa retornar uma tarefa Task<>
    public static async Task<IResult> Action(EmployeeRequest employeeRequest,HttpContext http, UserManager<IdentityUser> userManager){
        var userId = http.User.Claims.First(c=> c.Type == ClaimTypes.NameIdentifier).Value;
        var newUser = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email};
        var result = await userManager.CreateAsync(newUser,employeeRequest.Password);

        if(!result.Succeeded){
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
        }

        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.Name),
            new Claim("CreatedBy",userId)
        };
        var claimResult = await userManager.AddClaimsAsync(newUser, userClaims);
       
        if(!claimResult.Succeeded){
        return Results.BadRequest(result.Errors.First());
        }

        return Results.Created($"/EmployeePost/{newUser.Id}",newUser.Id);
    }

}