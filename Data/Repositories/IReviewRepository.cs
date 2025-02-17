using Bicycle.Models;

namespace Bicycle.Data.Repositories;

public interface IReviewRepository
{
    void AddReview(Review review);
    Review GetReview(int id);
    FileModel GetFile(int fileId);
    IEnumerable<Review> GetAllReviews();
    IEnumerable<Review> GetFindReviews(string search);
    void Save();
    void Edit(Review review);
    void Delete(Review review);
    void DeleteFile(FileModel review);
    void Detach(Review review);
}