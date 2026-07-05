using OrderManagementUserService.Models;
using static BCrypt.Net.BCrypt;

public static class PasswordValidation
{
    extension(Password password)
    {
        public bool Match(string passwordPlain)
            => Verify(passwordPlain, password.Value);
        
        public static Password CreatePasswordFromString(string passwordPlain)
            => new(HashPassword(passwordPlain));
    }
}