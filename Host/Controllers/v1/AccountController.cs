using Auth1796.Core.Application.DTOs;
using Auth1796.Core.Application.DTOs.PasswordDTO;
using Auth1796.Core.Application.Responses;
using Auth1796.Core.Application.Services.Account.Interfaces;
using Auth1796.Core.Application.Wrapper;
using Auth1796.Infrastructure.Auth.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Auth1796.Host.Controllers.v1;

/// <summary>
/// Manages account-related operations such as login, password management, and email confirmation.
/// </summary>
public class AccountController : VersionedApiController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="request">Login credentials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK with token or 400/500 on error.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Authenticate user and return JWT token.")]
    [ProducesResponseType(typeof(Result<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _accountService.Login(request, GetIpAddress(), cancellationToken);
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
    /// Requests a password reset email for a user.
    /// </summary>
    /// <param name="request">Forgot password request details.</param>
    /// <returns>200 OK if email sent, 400/500 on error.</returns>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Request a password reset email for a user.")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            var result = await _accountService.ForgotPasswordAsync(request, GetOriginFromRequest());
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
    /// Resets a user's password.
    /// </summary>
    /// <param name="request">Reset password request details.</param>
    /// <returns>200 OK if reset, 400/500 on error.</returns>
    [HttpPost("reset-password")]
    [SwaggerOperation(Summary = "Reset a user's password.")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
    {
        try
        {
            var result = await _accountService.ResetPasswordAsync(request);
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
    /// Confirms a user's email address.
    /// </summary>
    /// <param name="userEmail">User's email address.</param>
    /// <param name="code">Confirmation code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if confirmed, 400/500 on error.</returns>
    [HttpGet("confirm-email")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Confirm email address for a user.")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userEmail, [FromQuery] string code, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _accountService.ConfirmEmailAsync(userEmail, code, cancellationToken);
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
    /// Changes the password of the currently logged in user.
    /// </summary>
    /// <param name="model">Change password request details.</param>
    /// <returns>200 OK if changed, 400/500 on error.</returns>
    [HttpPut("change-password")]
    [SwaggerOperation(Summary = "Change password of currently logged in user.")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest model)
    {
        try
        {
            if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                return Unauthorized(Problem(detail: "User is not authenticated."));
            }
            var result = await _accountService.ChangePasswordAsync(model, userId);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(Problem(detail: result.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, Problem(detail: ex.Message));
        }
    }

    private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}
