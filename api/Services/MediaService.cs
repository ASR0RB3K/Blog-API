using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using api.Data;
using api.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace api.Services
{
    public class MediaService : IMediaService
    {
        private readonly BlogContext _context;
        private readonly ILogger<MediaService> _logger;

        public MediaService(BlogContext context, ILogger<MediaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _context.Medias.AnyAsync(m => m.Id == id);

        public async Task<bool> ExistsAsync(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                if (await ExistsAsync(id))
                {
                    return false;
                }
            }
            return true;
        }

        public Task<Media> GetAsync(Guid id)
            => _context.Medias.FirstOrDefaultAsync(m => m.Id == id);

        public Task<List<Media>> GetAllAsync()
            => _context.Medias.ToListAsync();

        public async Task<List<Media>> GetAllAsync(IEnumerable<Guid> ids)
        {
            var medias = new List<Media>();
            foreach (var id in ids)
            {
                medias = _context.Medias.Where(m => m.Id == id).ToList();
            }
            return medias;
        }

        public async Task<(bool IsSuccess, Exception exception)> InsertAsync(List<Media> media)
        {
            try
            {
                await _context.Medias.AddRangeAsync(media);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Media created in db.");
                return(true, null);
            }

            catch(Exception e)
            {
                _logger.LogInformation($"Media creation failed: {e.Message}");
                return(false, e);
            }
        }

        public async Task<(bool IsSuccess, Exception exception)> DeleteAsync(Guid id)
        {
            var media = await GetAsync(id);
            if(media is default(Media))
            {
                return(false, new Exception("Not found."));
            }

            try
            {
                _context.Medias.Remove(media);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Media deleted from db.");
                return(true, null);
            }

            catch(Exception e)
            {
                _logger.LogInformation("Media deleting in db is failde.");
                return(false, e);
            }
        }
    }
}