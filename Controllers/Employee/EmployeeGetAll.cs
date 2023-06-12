using AppRequest.Repository.Data;
using Microsoft.AspNetCore.Authorization;

namespace AppRequest.Controllers.Employee;
public class EmployeeGetAll{
     public static string Template => "/employees";///employees?page=1&rows=10
     public static string[] Methods => new String[] { HttpMethod.Get.ToString()};
     public static Delegate Handle => Action;

   [AllowAnonymous]
    public static  async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithClaimName query){
     
        if (rows == null) rows = 1000;
        if (page == null) page = 1;
        var result = await query.Execute(page.Value, rows.Value);
        return Results.Ok(result);
    }

}