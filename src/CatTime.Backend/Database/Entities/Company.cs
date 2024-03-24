using CatTime.Shared;

namespace CatTime.Backend.Database.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; }
    public List<Employee> Employees { get; set; }

    public CompanyDTO ToDTO()
    {
        return new CompanyDTO
        {
            Id = this.Id,
            Name = this.Name
        };
    }
}

