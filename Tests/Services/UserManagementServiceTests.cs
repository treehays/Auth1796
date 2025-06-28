using Auth1796.Core.Application.DTOs.UserManagmentDTO;
using Auth1796.Core.Application.Repositories;
using Auth1796.Core.Application.Repositories.Common.Interfaces;
using Auth1796.Core.Application.Services.UserManagement.Implementation;
using Auth1796.Core.Application.Services.UserManagement.Interface;
using Auth1796.Core.Application.Wrapper;
using Auth1796.Core.Domain.Entities;
using Auth1796.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Auth1796.Tests.Services;

public class UserManagementServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IUserManagementService _userManagementService;
    private readonly Mock<IGenericRepository<ApplicationUser>> _genericRepoMock;

    public UserManagementServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        var roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
        _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
            roleStoreMock.Object, null, null, null, null);

        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _genericRepoMock = new Mock<IGenericRepository<ApplicationUser>>();

        _unitOfWorkMock.Setup(x => x.repository<ApplicationUser>())
            .Returns(_genericRepoMock.Object);

        _userManagementService = (IUserManagementService)Activator.CreateInstance(
            typeof(UserManagementService),
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
            null,
            new object[]
            {
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object
            },
            null
        );
    }

    [Fact]
    public async Task RegisterUserAsync_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            RoleName = "Admin"
        };

        var role = new ApplicationRole("Admin", "Admin");
        _roleManagerMock.Setup(x => x.FindByNameAsync(request.RoleName))
            .ReturnsAsync(role);

        _genericRepoMock.Setup(x => x.Exist(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.RoleName))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userManagementService.RegisterUserAsync(request, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingEmail_ReturnsFailure()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "existing@example.com",
            RoleName = "Admin"
        };

        _genericRepoMock.Setup(x => x.Exist(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _userManagementService.RegisterUserAsync(request, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("User already exist", result.Message);
    }

    [Fact]
    public async Task RegisterUserAsync_WithInvalidRole_ReturnsFailure()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            RoleName = "InvalidRole"
        };

        _genericRepoMock.Setup(x => x.Exist(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _roleManagerMock.Setup(x => x.FindByNameAsync(request.RoleName))
            .ReturnsAsync((ApplicationRole)null);

        // Act
        var result = await _userManagementService.RegisterUserAsync(request, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("select a valid user role", result.Message);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = "test@example.com";
        var user = new ApplicationUser
        {
            Id = userId,
            Email = userId,
            FirstName = "Test",
            LastName = "User"
        };

        _genericRepoMock.Setup(x => x.FindOneAsync(
            It.IsAny<Expression<Func<ApplicationUser, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _userManagementService.GetUserByIdAsync(userId, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.Equal(userId, result.Data.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithInvalidId_ReturnsFailure()
    {
        // Arrange
        var userId = "nonexistent@example.com";

        _genericRepoMock.Setup(x => x.FindOneAsync(
            It.IsAny<Expression<Func<ApplicationUser, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await _userManagementService.GetUserByIdAsync(userId, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("user not found", result.Message);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ToggleStatusAsync_ReturnsExpectedResult(bool activate)
    {
        // Arrange
        var userId = "test@example.com";
        var user = new ApplicationUser
        {
            Id = userId,
            Email = userId,
            IsActive = !activate
        };

        var request = new ToggleUserStatusRequest
        {
            UserId = userId,
            ActivateUser = activate
        };

        _genericRepoMock.Setup(x => x.FindOneAsync(
            It.IsAny<Expression<Func<ApplicationUser, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.IsInRoleAsync(user, It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var result = await _userManagementService.ToggleStatusAsync(request, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(activate, user.IsActive);
    }

    [Fact]
    public async Task UpdateUserRole_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var userId = "test@example.com";
        var roleName = UserRoles.ProductManager;
        var user = new ApplicationUser { Id = userId, Email = userId };
        var role = new ApplicationRole(roleName.ToString(), roleName.ToString());

        var request = new UpdateUserRoleRequestModel
        {
            UserId = userId,
            RoleName = roleName
        };

        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>());
        _userManagerMock.Setup(x => x.AddToRoleAsync(user, roleName.ToString())).ReturnsAsync(IdentityResult.Success);
        _roleManagerMock.Setup(x => x.FindByNameAsync(roleName.ToString())).ReturnsAsync(role);

        // Act
        var result = await _userManagementService.UpdateUserRole(request);

        // Assert
        Assert.True(result.Succeeded);
    }
}