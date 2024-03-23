namespace CatTime.Shared;

public class EmployeeDTO : BaseEntity
{
    public int CompanyId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public string PasswordHash { get; set; }

    public EmployeeRole Role { get; set; }
}