﻿using BookApi.Models;
using BookAPI.Models.Filters;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IFeedbackRepository : IManagable<Feedback>
    {
        Task<PaginatedResult<Feedback>> GetAllAsync(int pageNumber, int pageSize, FeedbackFilter? filter);
        Task DeleteAsync(Guid id);
    }
}
