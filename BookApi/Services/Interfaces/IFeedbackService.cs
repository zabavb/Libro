﻿using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;

namespace BookAPI.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<PaginatedResult<FeedbackDto>> GetFeedbacksAsync(int pageNumber, int pageSize, FeedbackFilter? filter, FeedbackSort? sort);
        Task<FeedbackDto> GetFeedbackByIdAsync(Guid id);
        Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto FeedbackDto);
        Task<FeedbackDto> UpdateFeedbackAsync(Guid id, FeedbackDto FeedbackDto);
        Task<bool> DeleteFeedbackAsync(Guid id);
        Task<CollectionSnippet<FeedbackDetailsSnippet>> GetAllByUserId(Guid id, int pageNumber = 1);

    }
}
