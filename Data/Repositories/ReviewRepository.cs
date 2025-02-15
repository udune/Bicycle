using Bicycle.Models;

namespace Bicycle.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddReview(Review review)
    {
        _context.Reviews.Add(review);
    }

    public Review GetReview(int id)
    {
        var review = _context.Reviews
            .FirstOrDefault(x => x.Id == id);
        return review;
    }

    public IEnumerable<Review> GetAllReviews()
    {
        var reviews = _context.Reviews;
        return reviews;
    }
    
    public void Save()
    {
        _context.SaveChanges();
    }

    public void Edit(Review review)
    {
        _context.Update(review);
    }

    public void Delete(Review review)
    {
        _context.Remove(review);
    }
}