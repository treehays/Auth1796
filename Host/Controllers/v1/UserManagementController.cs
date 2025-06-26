using Auth1796.Core.Application.DTOs.UserManagmentDTO;
using Auth1796.Core.Application.Paging;
using Auth1796.Core.Application.Services.UserManagement.Interface;
using Auth1796.Core.Application.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Auth1796.Host.Controllers.v1;

[AllowAnonymous]
/// <summary>
/// Manages user-related operations such as registration, retrieval, role assignment, and status toggling.
/// </summary>
public class UserManagementController : VersionedApiController
{
    private readonly IUserManagementService _userService;

    public UserManagementController(IUserManagementService userManagementService)
    {
        _userService = userManagementService;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">User registration details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>201 Created with result or error details.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new user.", Description = "Endpoint to register back office admins.")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.RegisterUserAsync(request, cancellationToken);
            if (result.Succeeded)
                return CreatedAtAction(nameof(GetUserById), new { id = request.Email }, result);
            return BadRequest(Problem(detail: result.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(detail: ex.Message));
        }
    }

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK with user or 404 if not found.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get user by ID.", Description = "Fetch a user by their unique identifier.")]
    [ProducesResponseType(typeof(Result<GetUserResponseModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById([FromRoute] string id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.GetUserByIdAsync(id, cancellationToken);
            if (result.Data != null)
                return Ok(result);
            return NotFound(Problem(detail: $"User with id {id} not found."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(detail: ex.Message));
        }
    }

    /// <summary>
    /// Gets a paginated list of users.
    /// </summary>
    /// <param name="request">Pagination and filter parameters.</param>
    /// <returns>200 OK with paginated users.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Fetch paginated list of users.")]
    [ProducesResponseType(typeof(Result<PaginatedList<GetUserResponseModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequestModel request)
    {
        try
        {
            var result = await _userService.GetUsersAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(detail: ex.Message));
        }
    }

    /// <summary>
    /// Gets all user roles.
    /// </summary>
    /// <returns>200 OK with list of roles.</returns>
    [HttpGet("roles")]
    [SwaggerOperation(Summary = "Fetch user roles.")]
    [ProducesResponseType(typeof(Result<IEnumerable<GetRoleResponseModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var result = await _userService.GetRolesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(detail: ex.Message));
        }
    }

    /// <summary>
    /// Updates a user's role.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="request">Role update details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if updated, 400 if failed.</returns>
    [HttpPut("{id}/role")]
    [SwaggerOperation(Summary = "Assign or update a user's assigned role.")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUserRole([FromRoute] string id, [FromBody] UpdateUserRoleRequestModel request, CancellationToken cancellationToken)
    {
        try
        {
            request.UserId = id;
            var result = await _userService.UpdateUserRole(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(Problem(detail: result.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(detail: ex.Message));
        }
    }

    /// <summary>
    /// Toggles a user's active status.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="request">Status toggle details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if toggled, 400 if failed.</returns>
    [HttpPatch("{id}/status")]
    [SwaggerOperation(Summary = "Toggle a user's active status.")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ToggleStatusAsync([FromRoute] string id, [FromBody] ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            request.UserId = id;
            var result = await _userService.ToggleStatusAsync(request, cancellationToken);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(Problem(detail: result.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(detail: ex.Message));
        }
    }
}
