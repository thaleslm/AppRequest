using AppRequest.Repository.Data;
using AppRequest.Service.Products;


namespace AppRequest.Controllers.Categories;

public class CategoryGetAll{
     public static string Template => "/categories";
     public static string[] Methods => new String[] { HttpMethod.Get.ToString()};
     public static Delegate Handle => Action;

    public static IResult Action( ApplicationDbContext context){
        var Categories = context.Categories.ToList();
        var response = Categories.Select(c => new CategoryResponse { Id = c.Id, Name = c.Name , Active = c.Active});
     
        
        return Results.Ok(response);
    }

}