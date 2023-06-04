using AppRequest.Repository.Data;
using AppRequest.Service.Products;


namespace AppRequest.Controllers.Categories;

public class CategoryPost{
     public static string Template => "/categories";
     public static string[] Methods => new String[] { HttpMethod.Post.ToString()};
     public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context){
        var category = new Category(categoryRequest.Name)
        {
            
            CreatedBy = "test",
            CreatedOn = DateTime.Now,
            EditedBy ="test",
            EditedOn = DateTime.Now,

        };
        if(!category.IsValid){
            return Results.BadRequest(category.Notifications);
        }
        context.Categories.Add(category);
        context.SaveChanges();
        return Results.Created($"/categories/{category.Id}",category);
    }

}