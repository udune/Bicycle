using Bicycle.Models;

namespace Bicycle.ViewModels;

public class ReviewViewModel
{
    public Review Review { get; set; } = new Review();
    public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
}