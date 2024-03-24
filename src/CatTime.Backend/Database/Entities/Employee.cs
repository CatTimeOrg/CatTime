using CatTime.Shared;
using CatTime.Shared.Routes.Employees;

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

    public bool IsActive { get; set; }

    public string Department { get; set; }

    public string PhoneNumber { get; set; }

    public Employee()
    {
    }

    public Employee(CreateEmployeeRequest request, int companyId)
    {
        CompanyId = companyId;
        FirstName = request.FirstName;
        LastName = request.LastName;
        EmailAddress = request.EmailAddress;
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        Role = request.Role;
        IsActive = true;
        Department = request.Department;
        PhoneNumber = request.PhoneNumber;
    }

    public EmployeeDTO ToDTO()
    {
        return new EmployeeDTO
        {
            Id = this.Id,
            CompanyId = this.CompanyId,
            FirstName = this.FirstName,
            LastName = this.LastName,
            EmailAddress = this.EmailAddress,
            Role = this.Role,
            IsActive = this.IsActive,
            Department = this.Department,
            PhoneNumber = this.PhoneNumber            
        };
    }

    public void Update(UpdateEmployeeRequest request)
    {
        this.FirstName = request.FirstName != null ? request.FirstName : this.FirstName;
        this.LastName = request.LastName != null ? request.LastName : this.LastName;
        this.EmailAddress = request.EmailAddress != null ? request.EmailAddress : this.EmailAddress;
        this.PasswordHash = request.Password != null ? BCrypt.Net.BCrypt.HashPassword(request.Password) : this.PasswordHash;
        this.Role = request.Role.HasValue ? request.Role.Value : this.Role;
        this.IsActive = request.IsActive.HasValue ? request.IsActive.Value : this.IsActive;
        this.Department = request.Department != null ? request.Department : this.Department;
        this.PhoneNumber = request.PhoneNumber != null ? request.PhoneNumber : this.PhoneNumber;
    }
}

