namespace CatTime.Shared;

public class EmployeeDTO : BaseEntity
{
    public int CompanyId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public EmployeeRole Role { get; set; }

    public bool IsActive { get; set; }

    public string Department { get; set; }

    public string PhoneNumber { get; set; }
}