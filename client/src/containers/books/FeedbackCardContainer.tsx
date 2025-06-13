import React, { useEffect, useState } from 'react';
import FeedbackCard from '@/components/book/FeedbackCard';
import { FeedbackCard as FeedbackCardType } from '@/types/types/book/FeedbackCard';
import { fetchFeedbacksService } from '@/services/feedbackService';
import { PaginatedResponse } from '@/types';

const FeedbackCardContainer: React.FC = () => {
  const [feedbacks, setFeedbacks] = useState<FeedbackCardType[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  // Fetch 4 feedbacks on component mount
  useEffect(() => {
    const fetchFeedbacks = async () => {
      try {
        setLoading(true);
        const response = await fetchFeedbacksService(
          1, 
          4, 
        );

        if (response.error) {
          throw new Error(response.error);
        }

        const mappedFeedbacks: FeedbackCardType[] =
          response.data?.items.map((item) => ({
            feedbackId: item.feedbackId,
            comment: item.comment,
            rating: item.rating,
            date: new Date(item.date),
            userDisplayData: {
              userName: item.userName ?? '',
              userImageUrl: item.bookImageUrl || undefined,
            },
          })) || [];

        setFeedbacks(mappedFeedbacks);
      } catch (err) {
        setError('Failed to load reviews.');
      } finally {
        setLoading(false);
      }
    };

    fetchFeedbacks();
  }, []);

  const displayedFeedbacks = feedbacks.slice(0, 4);

  return (
    <div className="flex flex-col gap-5 px-16 py-5">
      <h2 className="text-2xl font-semibold">Latest Reviews</h2>
      {loading ? (
        <p>Loading reviews...</p>
      ) : error ? (
        <p className="text-red-500">{error}</p>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-5">
          {displayedFeedbacks.length > 0 ? (
            displayedFeedbacks.map((feedback) => (
              <FeedbackCard key={feedback.feedbackId} feedback={feedback} />
            ))
          ) : (
            <p>No reviews available.</p>
          )}
        </div>
      )}
    </div>
  );
};

export default FeedbackCardContainer;