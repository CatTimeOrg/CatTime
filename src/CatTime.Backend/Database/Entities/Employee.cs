using CatTime.Shared;

namespace CatTime.Backend.Database.Entities;

public class Employee : BaseEntity
{
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    public List<WorkingTime> WorkingTimes { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string EmailAddress { get; set; }
    public string PasswordHash { get; set; }

    public EmployeeRole Role { get; set; }
}

