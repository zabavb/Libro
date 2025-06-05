using AutoMapper;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services.Interfaces;
using Library.Common;
using Library.DTOs.UserRelated.User;

namespace BookAPI.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogger<FeedbackService> _logger;
        private readonly IBookService _bookService;

        public FeedbackService(IMapper mapper, IFeedbackRepository feedbackRepository, ILogger<FeedbackService> logger,
            IBookService bookService)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
            _logger = logger;
            _bookService = bookService;
        }

        public async Task<PaginatedResult<FeedbackAdminCard>> GetAllAsync(
            int pageNumber,
            int pageSize,
            FeedbackFilter? filter,
            FeedbackSort? sort)
        {
            var feedbacks = await _feedbackRepository.GetAllAsync(
                pageNumber,
                pageSize,
                filter,
                sort);

            if (feedbacks == null || feedbacks.Items == null)
            {
                _logger.LogWarning("No feedback found");
                return new PaginatedResult<FeedbackAdminCard>
                {
                    Items = new List<FeedbackAdminCard>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }

            _logger.LogInformation("Successfully found feedback");
            return new PaginatedResult<FeedbackAdminCard>
            {
                Items = feedbacks.Items,
                TotalCount = feedbacks.TotalCount,
                PageNumber = feedbacks.PageNumber,
                PageSize = feedbacks.PageSize
            };
        }


        public async Task<FeedbackDto> GetByIdAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);

            if (feedback == null)
            {
                _logger.LogWarning($"No feedback with id {id}");
                return null;
            }

            _logger.LogInformation($"Successfully found feedback with id {id}");
            return _mapper.Map<FeedbackDto>(feedback);
        }

        public async Task<ICollection<FeedbackForUserDetails>?> GetAllForUserDetailsAsync(Guid userId)
        {
            try
            {
                return await _feedbackRepository.GetAllForUserDetailsAsync(userId);
            }
            catch
            {
                return null;
            }
        }

        public async Task /*<FeedbackDto>*/ CreateAsync(FeedbackDto feedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackDto);

            try
            {
                await _feedbackRepository.CreateAsync(feedback);
                _logger.LogInformation($"Successfully created feedback with id {feedbackDto.FeedbackId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to create feedback. Error: {ex.Message}");
            }

            // return _mapper.Map<FeedbackDto>(feedback);
        }

        //TODO: Remove
        /*public async Task /*<FeedbackDto>#1# UpdateAsync(Guid id, FeedbackDto feedbackDto)
        {
            var existingfeedback = await _feedbackRepository.GetByIdAsync(id);

            if (existingfeedback == null)
            {
                _logger.LogWarning($"UpdateFeedbackAsync returns null");
                // return null;
            }

            try
            {
                _mapper.Map(feedbackDto, existingfeedback);
                await _feedbackRepository.UpdateAsync(existingfeedback);
                _logger.LogInformation($"Successfully updated author with id {feedbackDto.FeedbackId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to update feedback. Error: {ex.Message}");
            }

            // return _mapper.Map<FeedbackDto>(existingfeedback);
        }*/

        public async Task /*<bool>*/ DeleteAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);

            if (feedback == null)
            {
                _logger.LogWarning($"DeleteFeedbackAsync returns null");
                // return false;
            }

            try
            {
                await _feedbackRepository.DeleteAsync(id);
                _logger.LogInformation($"Successfully deleted feedback with id {id}");
                // return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to delete feedback. Error: {ex.Message}");
                // return false;
            }
        }

        public async Task<ICollection<FeedbackDto>> GetNumberOfFeedbacks(int amount, Guid bookId)
        {
            try
            {
                var feedbacks = await _feedbackRepository.GetNumberOfFeedbacks(amount, bookId);
                return _mapper.Map<List<FeedbackDto>>(feedbacks);
            }
            catch
            {
                return null;
            }
        }
    }
}