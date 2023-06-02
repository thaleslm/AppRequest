namespace AppRequest.Controllers.Categories;

    public class CategoryRequest
    {
    public string Name {get;set;}
    public bool Active {get;set;}
    public string CreatedBy  {get;set;}
    public DateTime CreatedOn { get;set;}
    public string EditedBy  {get;set;}
    public DateTime EditedOn { get;set;}
    }
