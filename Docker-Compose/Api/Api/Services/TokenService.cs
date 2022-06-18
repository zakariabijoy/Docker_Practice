using Api.Dtos;
using Api.Interfaces;
using Api.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Api.Helper.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.Helper;
using System.Security.Cryptography;

namespace Api.Services;

public class TokenService : ITokenService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;
    private readonly ITokenRepository _tokenRepository;
    private readonly SymmetricSecurityKey _key;
    private User user;
    private Token newRefreshToken;
    private Token refreshToken;

    public TokenService(IUserService userService, IConfiguration config, ITokenRepository tokenRepository)
    {
        _userService = userService;
        _config = config;
        _tokenRepository = tokenRepository;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecuritySettings:JwtSettings:key"]));
    }
    public async Task<TokenService> UserValidate(TokenRequestDto requestDto)
    {
        var userFromDb = await _userService.GetByNameAsync(requestDto.UserName);
        if (userFromDb == null) throw new AppException("Invalid User Name"); 

        using var hmac = new HMACSHA512( Convert.FromBase64String(userFromDb.Password_Salt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestDto.Password));

        if(Convert.ToBase64String(computedHash) != userFromDb.Password_Hash) throw new AppException("Invalid Password");

        user = userFromDb;

        return this;
    }

    public async Task<TokenService> CreateRefreshtoken()
    {
        newRefreshToken = new Token()
        {
            User_Id = user.User_Id,
            Value = Guid.NewGuid().ToString("N"),
            Created_Date = DateTime.Now.DateTimeToUnix(),
            Expiry_Time = DateTime.Now.AddMinutes(double.Parse(_config["SecuritySettings:JwtSettings:refreshTokenExpirationInMinutes"])).DateTimeToUnix()
        };

        await _tokenRepository.DeleteAsync(user.User_Id);
        await _tokenRepository.AddAsync(newRefreshToken);

        return this;
    }

    public async Task<TokenResponseDto> CreateAccessToken()
    {
        double tokenExpiryTime = double.Parse(_config["SecuritySettings:JwtSettings:tokenExpirationInMinutes"]);

        var key = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);


        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>();


        claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.User_Name));
        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.User_Id.ToString()));

      
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = key,
            Expires = DateTime.Now.AddMinutes(tokenExpiryTime)
        };

        var newtoken = tokenHandler.CreateToken(tokenDescriptor);

        var encodedToken = tokenHandler.WriteToken(newtoken);

        return new TokenResponseDto()
        {
            Token = encodedToken,
            Expiration = newtoken.ValidTo,
            RefreshToken = newRefreshToken.Value,
            RefreshTokenExpiration = newRefreshToken.Expiry_Time.UnixToDateTime(),
            UserName = user.User_Name,
            FirstName = user.First_Name,
            LastName = user.Last_Name,
            Email = user.Email,

        };
    }

    public async Task<TokenService> RefreshTokenValidate(TokenRequestDto tokenRequestDto)
    {
        var refreshTokenFromDb = await _tokenRepository.GetByRefreshtokenAsync(tokenRequestDto.RefreshToken);
        if (refreshTokenFromDb == null) throw new UnauthorizedAccessException("Refresh Token Invalid");

        var refreshTokenExpiryTime = refreshTokenFromDb.Expiry_Time.UnixToDateTime();
        if (refreshTokenExpiryTime < DateTime.Now) throw new UnauthorizedAccessException("Refresh Token Expired");

        refreshToken = refreshTokenFromDb;

        return this;
    }

    public async Task<TokenService> UserValidateById()
    {
        var userFromDb = await _userService.GetByIdAsync(refreshToken.User_Id);
        if (userFromDb == null) throw new UnauthorizedAccessException("User Invalid");

        user = userFromDb;

        return this;
    }

}
