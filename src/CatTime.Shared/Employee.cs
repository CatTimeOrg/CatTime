namespace CatTime.Shared;

public class Employee : BaseEntity
{
    public int CompanyId { get; set; }

    public string EmailAddress { get; set; }
    public string PasswordHash { get; set; }

    public EmployeeRole Role { get; set; }
}

