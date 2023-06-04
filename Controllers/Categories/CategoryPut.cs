using AppRequest.Repository.Data;
using AppRequest.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace AppRequest.Controllers.Categories;

public class CategoryPut{
     public static string Template => "/categories/{id:guid}";
     public static string[] Methods => new String[] { HttpMethod.Put.ToString()};
     public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id ,CategoryRequest categoryRequest, ApplicationDbContext context){
       var categories = context.Categories.Where(c => c.Id == id).FirstOrDefault();
       if(categories == null){
        return Results.NotFound("Categoria não encontrada!!");
       }
       categories.EditInfo(categoryRequest.Name,categoryRequest.Active);
        if(!categories.IsValid){
            return Results.ValidationProblem(categories.Notifications.ConvertToProblemDetails());
        }

        context.SaveChanges();
        return Results.Ok(categories);
    }

}