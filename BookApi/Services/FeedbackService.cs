using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using BookApi.Services;
using BookAPI.Repositories;
using BookAPI.Services;
using Microsoft.EntityFrameworkCore;


namespace FeedbackApi.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(IMapper mapper, IFeedbackRepository feedbackRepository, ILogger<FeedbackService> logger)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbacksAsync()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();

            if (feedbacks == null || feedbacks.Count == 0)
            {
                _logger.LogWarning("No feedback found");
                return [];
            }
            _logger.LogInformation("Successfully found feedback");
            return _mapper.Map<List<FeedbackDto>>(feedbacks);
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

    }

}