﻿namespace Auth1796.Core.Application.DTOs.PasswordDTO;

public class ResetPasswordRequest
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Token { get; set; }
}