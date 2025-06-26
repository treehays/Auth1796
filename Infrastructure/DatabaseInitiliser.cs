using Auth1796.Core.Application.Repositories;
using Auth1796.Core.Application.Utilities;
using Auth1796.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth1796.Infrastructure;

public class DatabaseInitiliser : IDatabaseInitiliser
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DatabaseInitiliser(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedDatas()
    {
        List<string> enumRoles = Enum.GetNames(typeof(UserRoles)).ToList();
        await SeedRole(enumRoles);
        await SeedUser(enumRoles);
        await AddApiUser();
    }

    //step1
    private async Task SeedRole(List<string> enumRoles)
    {
        foreach (string item in enumRoles)
        {
            await AddRoleAsync(item);
        }
    }

    //Step2
    private async Task SeedUser(List<string> enumRoles)
    {
        foreach (string item in enumRoles)
        {
            var result = await AddMockUserWithRoleAsync(item);
        }
    }
    private async Task<IdentityResult> AddMockUserWithRoleAsync(string role)
    {
        var mockUser = MockUser(role);
        var result = await _userManager.CreateAsync(mockUser, "P@$$w0rd!");
        if (result.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(mockUser, role);
            if (!roleResult.Succeeded)
            {
                return roleResult;
            }
        }
        return result;
    }

    private async Task AddRoleAsync(string role)
    {
        var appRole = new ApplicationRole(role, role);
        await _roleManager.CreateAsync(appRole);
    }
    //Mocked User

    private ApplicationUser MockUser(string role)
    {
        string email = $"{role}@Auth1796.ng".ToUpperInvariant();

        return new ApplicationUser
        {
            UserName = email,
            Email = email,
            Status = Status.Active.ToFriendlyString(),
            IsActive = true,
            IsDissabled = false,
            PhoneNumber = "0812345678",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            NormalizedEmail = email,
            NormalizedUserName = email,
        };
    }
    private async Task AddApiUser()
    {
        var apiUser = new ApiUser()
        {
            Id = "aa3c6080-7b65-4256-8d3c-a54b2d78c26c",
            Email = "Auth1796@GMAIL.COM",
            Iv = "qS8hhySIh7vdEDyn",
            SecretKey = "VdFczdZRbBBbXh1n",
            ClientId = "Auth1796WEB",
        };
        await _unitOfWork.repository<ApiUser>().AddAsync(apiUser);
        await _unitOfWork.Complete();
    }
}