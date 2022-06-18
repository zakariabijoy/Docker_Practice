using Api.Data.Context;
using Api.Dtos;
using Api.Interfaces;
using Api.Models;
using Dapper;
using System.Data;

namespace Api.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _db;

    public UserRepository(DapperContext context) => _db = context.GetDbConnection();

    public Task<int> AddAsync(User entity) => throw new NotImplementedException();


    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public Task<List<User>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

    public async Task<List<UserDto>> GetAllUserAsync(PaginationParams pParams)
    {
        string sql = @"SELECT
                        COUNT(*) OVER() as TotalRowCount,
                        u.user_id as UserId,
                        u.first_name as FirstName,
                        u.last_name as LastName,
                        u.user_name as UserName,
                        u.email as Email
                        FROM dbo.users as u";

        if (pParams.SearchBy != null)
        {
            sql += @" WHERE u.user_name  like   '%' + @searchBy + '%'";
        }

        sql += @" order by u.user_id
				    OFFSET ((@pageNumber)*@pageSize) ROWS FETCH FIRST @pageSize ROW ONLY;";

        var users = await _db.QueryAsync<UserDto>(sql, new { pageNumber = pParams.PageNumber, pageSize = pParams.pageSize, searchBy = pParams.SearchBy });

        return users.ToList();
    }

    public async Task<User> GetByNameAsync(string name)
    {
        var sql = "select * from dbo.users  where user_name = @name";
        var user = await _db.QueryAsync<User>(sql, new { @name = name });
        return user.FirstOrDefault();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var sql = "select * from dbo.users  where user_id = @id";
        var user = await _db.QueryAsync<User>(sql, new { id = id });
        return user.FirstOrDefault();
    }

    public Task<int> UpdateAsync(User entity) => throw new NotImplementedException();

    public async Task<int> GetTotalCountAsync(string searchBy)
    {
        string countSql = @"SELECT COUNT (DISTINCT user_id)
                            FROM dbo.users u";
        if (searchBy != null)
        {
            countSql += @" WHERE u.user_name  like   '%' + @searchBy + '%';";
        }

        var totalCount = await _db.QueryAsync<int>(countSql, new { @searchBy = searchBy });
        return totalCount.FirstOrDefault();
    }


}
