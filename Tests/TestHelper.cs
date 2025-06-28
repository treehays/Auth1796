using Auth1796.Core.Application.DTOs.UserManagmentDTO;
using Auth1796.Core.Domain.Entities;

namespace Auth1796.Tests;

public static class TestHelper
{
    public static CreateUserRequest CreateValidUserRequest()
    {
        return new CreateUserRequest
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            RoleName = "Admin"
        };
    }

    public static ApplicationUser CreateValidApplicationUser()
    {
        return new ApplicationUser
        {
            Id = "test@example.com",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            UserName = "test@example.com",
            IsActive = true,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            Status = "Active"
        };
    }
}