using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories;
using BookAPI.Services;
using Microsoft.EntityFrameworkCore;


namespace FeedbackApi.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IMapper mapper, IFeedbackRepository feedbackRepository)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbacksAsync()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();

            if (feedbacks == null || feedbacks.Count == 0)
            {
                return [];
            }

            return _mapper.Map<List<FeedbackDto>>(feedbacks);
        }


        public async Task<FeedbackDto> GetFeedbackByIdAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);

            if (feedback == null)
            {
                return null;
            }

            return _mapper.Map<FeedbackDto>(feedback);
        }
        public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto feedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackDto);

            await _feedbackRepository.CreateAsync(feedback);


            return _mapper.Map<FeedbackDto>(feedback);
        }
        public async Task<FeedbackDto> UpdateFeedbackAsync(Guid id, FeedbackDto feedbackDto)
        {
            var existingfeedback = await _feedbackRepository.GetByIdAsync(id);

            if (existingfeedback == null)
            {
                return null;
            }

            _mapper.Map(feedbackDto, existingfeedback);
            await _feedbackRepository.UpdateAsync(existingfeedback);

            return _mapper.Map<FeedbackDto>(existingfeedback);
        }

        public async Task<bool> DeleteFeedbackAsync(Guid id)
        {
            var feedback= await _feedbackRepository.GetByIdAsync(id);

            if (feedback == null)
            {
                return false;
            }

            await _feedbackRepository.DeleteAsync(id);
            return true;
        }

    }

}