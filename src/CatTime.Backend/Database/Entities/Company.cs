namespace CatTime.Backend.Database.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; }
    public List<Employee> Employees { get; set; }
}

