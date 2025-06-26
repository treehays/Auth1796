using Auth1796.Core.Application.DTOs.UserManagmentDTO;
using Auth1796.Core.Application.Paging;
using Auth1796.Core.Application.Repositories;
using Auth1796.Core.Application.Utilities;
using Auth1796.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth1796.Infrastructure.Persistence.Repositories;

internal class UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<PaginatedList<ApplicationUser>> GetUsersAsync(GetUsersRequestModel request)
    {
        IQueryable<ApplicationUser> users = Enumerable.Empty<ApplicationUser>().AsQueryable();

        // Get users based on the role
        if (request.RoleName == null)
        {
            users = _userManager.Users.AsQueryable();
        }
        else if (await _roleManager.RoleExistsAsync(request.RoleName.Value.ToFriendlyString()))
        {
            users = (await _userManager.GetUsersInRoleAsync(request.RoleName.Value.ToFriendlyString())).AsQueryable();
        }
        else
        {
            users = _userManager.Users.AsQueryable();
        }

        // Apply keyword filtering
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            var keywordLower = request.Keyword.ToLowerInvariant();
            Expression<Func<ApplicationUser, bool>> query = u =>
                u.FirstName.ToLowerInvariant().Contains(keywordLower) ||
                u.PhoneNumber.ToLowerInvariant().Contains(keywordLower) ||
                u.Email.ToLowerInvariant().Contains(keywordLower) ||
                u.LastName.ToLowerInvariant().Contains(keywordLower);

            users = users.Where(query);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            users = users.Where(x => !x.IsDissabled);

            users = request.SortBy.ToLowerInvariant() switch
            {
                "email" => request.SortDirection == "asc" ? users.OrderBy(u => u.Email) : users.OrderByDescending(u => u.Email),
                "isactive" => request.SortDirection == "asc" ? users.OrderBy(u => u.IsActive) : users.OrderByDescending(u => u.IsActive),
                "phonenumber" => request.SortDirection == "asc" ? users.OrderBy(u => u.PhoneNumber) : users.OrderByDescending(u => u.PhoneNumber),
                _ => request.SortDirection == "asc" ? users.OrderBy(u => u.FirstName) : users.OrderByDescending(u => u.FirstName),
            };
        }

        var totalItemsCount = await users.CountAsync();
        if (request.UsePaging)
        {
            var offset = (request.Page - 1) * request.PageSize;
            var result = await users.Skip(offset).Take(request.PageSize).ToListAsync();
            return result.ToPaginatedList(totalItemsCount, request.Page, request.PageSize);
        }
        else
        {
            var result = await users.ToListAsync();
            return result.ToPaginatedList(totalItemsCount, 1, totalItemsCount);
        }
    }
}