using Api.Dtos;
using Api.Interfaces;
using Api.Models;

namespace Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _config;

    public UserService(IUserRepository repository, IConfiguration config)
    {
        _repository = repository;
        _config = config;
    }

    public Task<int> AddAsync(User entity) => throw new NotImplementedException();




    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public Task<List<User>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

    public async Task<List<UserDto>> GetAllUserAsync(PaginationParams pParams) => await _repository.GetAllUserAsync(pParams);

    public async Task<User> GetByNameAsync(string name) => await _repository.GetByNameAsync(name);

    public async Task<User> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public Task<int> UpdateAsync(User entity) => throw new NotImplementedException();

    public async Task<int> GetTotalCountAsync(string searchBy) => await _repository.GetTotalCountAsync(searchBy);


}