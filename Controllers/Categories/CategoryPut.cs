using AppRequest.Repository.Data;
using AppRequest.Service.Products;
using Microsoft.AspNetCore.Mvc;

namespace AppRequest.Controllers.Categories;

public class CategoryPut{
     public static string Template => "/categories/{id}";
     public static string[] Methods => new String[] { HttpMethod.Put.ToString()};
     public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id ,CategoryRequest categoryRequest, ApplicationDbContext context){
       var categories = context.Categories.Where(c => c.Id == id).FirstOrDefault();
       if(categories != null){
        categories.Name = categoryRequest.Name;
        categories.Active = categoryRequest.Active;

       }

        context.SaveChanges();
        return Results.Ok(categories);
    }

}