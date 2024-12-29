using Library.Extensions;
using Library.Interfaces;

namespace UserAPI.Services
{
    public interface IUserService : IManagable<UserDto>
    {
        Task<PaginatedResult<UserDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort sort);
    }
}
