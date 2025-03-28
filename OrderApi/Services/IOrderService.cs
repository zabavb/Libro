﻿using Library.Common;
using Library.Interfaces;

namespace OrderApi.Services
{
    public interface IOrderService : IManageable<OrderDto,OrderDto>
    {
        Task<PaginatedResult<OrderDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort);
        Task DeleteAsync(Guid id);
    }
}
