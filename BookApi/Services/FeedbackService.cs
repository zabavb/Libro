using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using BookAPI.Services;
using Microsoft.EntityFrameworkCore;


namespace FeedbackApi.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;

        public FeedbackService(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbacksAsync()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();

            if (feedbacks == null || feedbacks.Count == 0)
            {
                return [];
            }

            return _mapper.Map<List<FeedbackDto>>(feedbacks);
        }


        public async Task<FeedbackDto> GetFeedbackByIdAsync(Guid id)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(b => b.Id == id);

            if (feedback == null)
            {
                return null;
            }

            return _mapper.Map<FeedbackDto>(feedback);
        }
        public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto feedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackDto);

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return _mapper.Map<FeedbackDto>(feedback);
        }
        public async Task<FeedbackDto> UpdateFeedbackAsync(Guid id, FeedbackDto feedbackDto)
        {
            var existingFeedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);

            if (existingFeedback == null)
            {
                return null;
            }

            existingFeedback.ReviewerName = feedbackDto.ReviewerName;
            existingFeedback.Comment = feedbackDto.Comment;
            existingFeedback.Rating = feedbackDto.Rating;
            existingFeedback.Date = feedbackDto.Date;
            existingFeedback.IsPurchased = feedbackDto.IsPurchased;

            await _context.SaveChangesAsync();

            return _mapper.Map<FeedbackDto>(existingFeedback);
        }

        public async Task<bool> DeleteFeedbackAsync(Guid id)
        {
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(b => b.Id == id);

            if (feedback == null)
            {
                return false;
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return true;
        }

    }

}