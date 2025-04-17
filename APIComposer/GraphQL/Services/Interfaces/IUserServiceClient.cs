using Library.Common;
using Library.DTOs.UserRelated.User;
using UserAPI.Models.Filters;
using UserAPI.Models.Sorts;

namespace APIComposer.GraphQL.Services.Interfaces
{
    public interface IUserServiceClient
    {
        Task<PaginatedResult<UserDto>> GetAllUsersAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            UserFilter? filter,
            UserSort? sort
        );

        Task<UserWithSubscriptionsDto> GetUserAsync(Guid id);
    }
}