using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entity;

namespace api.Services
{
    public interface IPostService
    {
        Task<(bool IsSuccess, Exception exception, Post post)> CreateAsync(Post post);
        Task<List<Post>> GetAllAsync();
        Task<Post> GetAsync(Guid id);
        Task<(bool IsSuccess, Exception exception, Post post)> UpdatePostAsync(Post post);
        Task<bool> ExistsAsync(Guid id);
        Task<(bool IsSuccess, Exception Exception)> DeleteAsync(Guid id);
    }
}