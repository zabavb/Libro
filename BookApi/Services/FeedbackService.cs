using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services;
using Library.Extensions;
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

        public async Task<PaginatedResult<FeedbackDto>> GetFeedbacksAsync(int pageNumber, int pageSize)
        {
            var feedbacks = await _feedbackRepository.GetAllAsync(pageNumber, pageSize);

            if (feedbacks == null || feedbacks.Items == null)
            {
                return new PaginatedResult<FeedbackDto>
                {
                    Items = new List<FeedbackDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }

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