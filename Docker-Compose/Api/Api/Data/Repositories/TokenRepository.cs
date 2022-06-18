using Api.Data.Context;
using Api.Dtos;
using Api.Helper.Extensions;
using Api.Interfaces;
using Api.Models;
using Dapper;
using System.Data;

namespace Api.Data.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly IDbConnection _db;

    public TokenRepository(DapperContext context) => _db = context.GetDbConnection();

    public async Task<int> AddAsync(Token entity)
    {
        entity.Last_Modified_Date = DateTime.Now.DateTimeToUnix();
        var sql = "INSERT INTO dbo.tokens (value, created_date, user_id, last_modified_date, expiry_time) VALUES(@value, @created_date, @user_id, @last_modified_date, @expiry_time); ";
        return await _db.ExecuteAsync(sql, new { value = entity.Value, created_date = entity.Created_Date, user_id = entity.User_Id, last_modified_date = entity.Last_Modified_Date, expiry_time = entity.Expiry_Time });
    }

    public async Task<int> DeleteAsync(int id)
    {
        var sql = @"delete from dbo.tokens where user_id = @id;";

        var deletedId = await _db.ExecuteAsync(sql, new { id });

        if (deletedId > 0)
        {
            return deletedId;
        }
        else
        {
            return 0;
        }
    }

    public Task<List<Token>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

    public Task<Token> GetByIdAsync(int id) => throw new NotImplementedException();

    public async Task<Token> GetByRefreshtokenAsync(string refreshToken)
    {
        var result = await _db.QueryAsync<Token>("select * from dbo.tokens where  value= @refreshToken", new { @refreshToken = refreshToken });
        return result.FirstOrDefault();
    }

    public Task<int> GetTotalCountAsync(string searchBy) => throw new NotImplementedException();

    public Task<int> UpdateAsync(Token entity) => throw new NotImplementedException();
}
