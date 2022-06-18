using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(ITokenService tokenService, IConfiguration config)
        {
            _tokenService = tokenService;
            _config = config;
        }

        [HttpPost("/api/user-login")]
        public async Task<IActionResult> Login([FromBody] TokenRequestDto requestDto)
        {
            if (!String.IsNullOrEmpty(requestDto.UserName) &&  !String.IsNullOrEmpty(requestDto.Password))
            {
                //Create New JWT and Refresh Token

                var aT = await _tokenService.UserValidate(requestDto).Result
                                        .CreateRefreshtoken().Result
                                        .CreateAccessToken();

                if (aT == null) return BadRequest("Generating a new token is failed");

                setTokenCookie(aT);

                return Ok(new { authToken = aT });
            }
            else
            {
                //Refresh JWT and Refresh Refresh Token
                requestDto.RefreshToken = Request.Cookies["refreshToken"];

                if(!String.IsNullOrEmpty(requestDto.RefreshToken))
                {
                    var aT = await _tokenService.RefreshTokenValidate(requestDto).Result
                                        .UserValidateById().Result
                                        .CreateRefreshtoken().Result
                                        .CreateAccessToken();

                    if (aT == null) return BadRequest("Refreshing token is failed");

                    setTokenCookie(aT);
                    return Ok(new { authToken = aT });
                }
                else
                {
                   return BadRequest("Refreshing token is failed");
                }
                
            }
        }

        [NonAction]
        private void setTokenCookie(TokenResponseDto aT)
        {
            //var cookieOptionsForToken = new CookieOptions
            //{
            //    HttpOnly = true,
            //    Expires = aT.Expiration
            //};
            var cookieOptionsForRefrreshToken = new CookieOptions
            {
                HttpOnly = true,
                Expires = aT.RefreshTokenExpiration
            };
            //Response.Cookies.Append("token", aT.Token, cookieOptionsForToken);
            Response.Cookies.Append("refreshToken", aT.RefreshToken, cookieOptionsForRefrreshToken);
        }
    }
}
