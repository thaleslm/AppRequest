using Flunt.Validations;
namespace AppRequest.Services.Products;
public class Category :Entity{
    public string Name {get; private set;}
    public bool Active {get;private set;}
  
    public Category(string name,string createdBy, string editedBy)
        {
          
            Active = true;
            Name = name;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            EditedBy = EditedBy;
            EditedOn = DateTime.Now;

            Validate();
        }

    public void Validate()
    {
          var contract = new Contract<Category>()
                .IsGreaterOrEqualsThan(Name,3, "Name")
                .IsNotNullOrEmpty(Name, "Name", "Nome é obrigatorio!")
                .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
                .IsNotNullOrEmpty(EditedBy, "EditedBy");
            AddNotifications(contract);  
    }

    public void EditInfo(string name, bool active){
        Active = active;
        Name = name;
        Validate();
    }

}
