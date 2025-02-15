using Bicycle.Models;

namespace Bicycle.Data.Repositories;

public interface IReviewRepository
{
    void AddReview(Review review);
    Review GetReview(int id);
    IEnumerable<Review> GetAllReviews();
    void Save();
    void Edit(Review review);
    void Delete(Review review);
}