namespace AppRequest.Service;

public abstract class Entity{

    public Entity(){
        //toda vez que estanciar as que herdaram Entity vai ter o id pronto
        Id = Guid.NewGuid();
    }
    public Guid Id {get; set;}
    public string Name {get;set;}
    public string CreatedBy  {get;set;}
    public DateTime CreatedOn { get;set;}
    public string EditedBy  {get;set;}
    public DateTime EditedOn { get;set;}
}