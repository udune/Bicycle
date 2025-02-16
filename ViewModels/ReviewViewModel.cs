using Bicycle.Models;

namespace Bicycle.ViewModels;

public class ReviewViewModel
{
    public Review Review { get; set; } = new Review();
    public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
    public ReviewTable ReviewTable { get; set; } = new ReviewTable(new List<Review>(), new ReviewTablePage(new List<Review>().Count, 1, 10));
}