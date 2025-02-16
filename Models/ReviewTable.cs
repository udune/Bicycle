namespace Bicycle.Models;

public class ReviewTable
{
    public ReviewTable(IEnumerable<Review>? reviews, ReviewTablePage? reviewTablePage)
    {
        Reviews = reviews;
        ReviewTablePage = reviewTablePage;
    }
    
    public IEnumerable<Review>? Reviews { get; }
    public ReviewTablePage? ReviewTablePage { get; }
}