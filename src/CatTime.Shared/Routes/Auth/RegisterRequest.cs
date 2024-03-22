namespace CatTime.Shared.Routes.Auth;

public class RegisterRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string EmailAddress { get; set; }
    public string Password { get; set; }
}