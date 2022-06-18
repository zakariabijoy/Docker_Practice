using Api.Models;

namespace Api.Interfaces;

public interface ITokenRepository : IBaseRepository<Token>
{
    Task<Token> GetByRefreshtokenAsync(string refreshToken);
}
