using Bicycle.Models;
using Microsoft.EntityFrameworkCore;

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
        if (review != null)
            review.File = GetFile(review.FileId);
        return review;
    }

    public FileModel GetFile(int fileId)
    {
        var file = _context.FileModel
            .FirstOrDefault(x => x.Id == fileId);
        return file;
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
        _context.Remove(review.File);
    }

    public void DeleteFile(FileModel file)
    {
        _context.Remove(file);
    }

    public void Detach(Review review)
    {
        _context.Entry(review).State = EntityState.Detached;
    }
}