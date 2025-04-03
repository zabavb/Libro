using Library.Common;
using Library.Interfaces;

namespace UserAPI.Services.Interfaces
{
    public interface IUserService : IManageable<Dto, DetailsDto>
    {
        Task<PaginatedResult<CardDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            Sort? sort
        );
    }
}