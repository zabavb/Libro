using BookAPI.Models;
using BookAPI.Services.Interfaces;

namespace BookAPI.Services
{
    public class CoverTypeService : ICoverTypeService
    {
        public Task<IEnumerable<string>> GetCoverTypesAsync()
        {
            var coverTypes = Enum.GetNames(typeof(CoverType));
            return Task.FromResult(coverTypes.AsEnumerable());
        }

        public Task<string> GetCoverTypeByIdAsync(int id)
        {
            var coverType = Enum.GetName(typeof(CoverType), id);
            return Task.FromResult(coverType ?? CoverType.OTHER.ToString());
        }
    }
}
