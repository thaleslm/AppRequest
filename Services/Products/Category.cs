using Flunt.Validations;
namespace AppRequest.Service.Products;
public class Category :Entity{
    public string Name {get;set;}
    public bool Active {get;set;}
  
    public Category(string name)
        {
            var contract = new Contract<Category>()
                .IsNotNull(Name, "Name", "Nome é obrigatorio!");
            AddNotifications(contract);
            
            Active = true;
            Name = name;
        }
}
