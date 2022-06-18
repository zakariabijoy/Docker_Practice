using Api.Dtos;
using Api.Services;

namespace Api.Interfaces;

public interface ITokenService
{
    public Task<TokenService> UserValidate(TokenRequestDto requestDto);
    public Task<TokenService> UserValidateById();
    public Task<TokenService> CreateRefreshtoken();
    public Task<TokenResponseDto> CreateAccessToken();
    public Task<TokenService> RefreshTokenValidate(TokenRequestDto tokenRequestDto);
}
