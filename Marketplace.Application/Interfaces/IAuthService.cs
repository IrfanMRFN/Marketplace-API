using Marketplace.Application.Common;
using Marketplace.Application.DTOs;

namespace Marketplace.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto request);
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto request);
}
