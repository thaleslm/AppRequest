namespace AppRequest.Services.Products;

public class Product : Entity{
    public string Name {get;set;}
    public Guid CategoryId {get;set;}
    public bool Active {get;set;} = true;
    public string Description {get;set;}
    public Category Category {get;set;}
     public bool HasStock {get;set;}
    
}