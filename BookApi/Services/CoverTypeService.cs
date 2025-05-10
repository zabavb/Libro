using BookAPI.Models;
using BookAPI.Services.Interfaces;
using Library.DTOs.Book;

namespace BookAPI.Services
{
    public class CoverTypeService : ICoverTypeService
    {
        public Task<IEnumerable<string>> GetAllAsync()
        {
            var coverTypes = Enum.GetNames(typeof(CoverType));
            return Task.FromResult(coverTypes.AsEnumerable());
        }

        public Task<string> GetByIdAsync(int id)
        {
            var coverType = Enum.GetName(typeof(CoverType), id);
            return Task.FromResult(coverType ?? CoverType.OTHER.ToString());
        }
    }
}
