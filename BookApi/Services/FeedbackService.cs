using AutoMapper;
using BookAPI;
using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services.Interfaces;
using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FeedbackApi.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogger<FeedbackService> _logger;
        private readonly IBookService _bookService;

        public FeedbackService(IMapper mapper, IFeedbackRepository feedbackRepository, ILogger<FeedbackService> logger, IBookService bookService)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
            _logger = logger;
            _bookService = bookService;
        }

        public async Task<PaginatedResult<FeedbackDto>> GetFeedbacksAsync(
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
                return new PaginatedResult<FeedbackDto>
                {
                    Items = new List<FeedbackDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            _logger.LogInformation("Successfully found feedback");
            return new PaginatedResult<FeedbackDto>
            {
                Items = _mapper.Map<ICollection<FeedbackDto>>(feedbacks.Items),
                TotalCount = feedbacks.TotalCount,
                PageNumber = feedbacks.PageNumber,
                PageSize = feedbacks.PageSize
            };
        }



        public async Task<FeedbackDto> GetFeedbackByIdAsync(Guid id)
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
        public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto feedbackDto)
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
            return _mapper.Map<FeedbackDto>(feedback);
        }
        public async Task<FeedbackDto> UpdateFeedbackAsync(Guid id, FeedbackDto feedbackDto)
        {
            var existingfeedback = await _feedbackRepository.GetByIdAsync(id);

            if (existingfeedback == null)
            {
                _logger.LogWarning($"UpdateFeedbackAsync returns null");
                return null;
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
            return _mapper.Map<FeedbackDto>(existingfeedback);
        }

        public async Task<bool> DeleteFeedbackAsync(Guid id)
        {
            var feedback= await _feedbackRepository.GetByIdAsync(id);

            if (feedback == null)
            {
                _logger.LogWarning($"DeleteFeedbackAsync returns null");
                return false;
            }
            try
            {
                await _feedbackRepository.DeleteAsync(id);
                _logger.LogInformation($"Successfully deleted feedback with id {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to delete feedback. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<CollectionSnippet<FeedbackDetailsSnippet>> GetAllByUserIdAsync(Guid id, int pageNumber)
        {
            try
            {
                var feedbackFilter = new FeedbackFilter { userId = id };
                var feedbacks = await _feedbackRepository.GetAllAsync(pageNumber, GlobalConstants.DefaultPageSize, feedbackFilter, null);

                var snippets = new List<FeedbackDetailsSnippet>();
                foreach (var feedback in feedbacks.Items)
                {
                    var book = await _bookService.GetBookByIdAsync(feedback.BookId);

                    var detailsSnippet = new FeedbackDetailsSnippet
                    {
                        HeadLabel = $"{book.Title} - {feedback.Id.ToString().Split('-')[4]}",
                        Comment = feedback.Comment,
                        Date = feedback.Date,
                        Rating = feedback.Rating,
                    };
                    snippets.Add(detailsSnippet);
                }

                return new CollectionSnippet<FeedbackDetailsSnippet>(false, snippets);
            }
            catch
            {
                return new CollectionSnippet<FeedbackDetailsSnippet>(true, new List<FeedbackDetailsSnippet>());
            }

                /*                    public string HeadLabel { get; set; }
                        public int Rating { get; set; }
                        public string Comment { get; set; }
                        public DateTime Date { get; set; }*/

            }

    }

}