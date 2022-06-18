using Api.Dtos;
using Api.Models;

namespace Api.Interfaces;

public interface IUserService : IBaseService<User>
{

    Task<User> GetByNameAsync(string name);
    Task<List<UserDto>> GetAllUserAsync(PaginationParams pParams);
}
