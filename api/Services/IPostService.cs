using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entity;

namespace api.Services
{
    public interface IPostService
    {
        Task<bool> ExistsAsync(Guid id);
        Task<Post> GetAsync(Guid id);
        Task<List<Post>> GetAllAsync();
        Task<List<Post>> GetJsonAsync();
        Task<(bool IsSuccess, Exception exception)> InsertAsync(Post post);
        Task<(bool IsSuccess, Exception exception)> UpdateAsync(Post post);
        Task<(bool IsSuccess, Exception exception)> DeleteAsync(Guid id);
    }
}