namespace Bicycle.Models;

public class ReviewTablePage
{
    public ReviewTablePage(int totalCount, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPage = (int) Math.Ceiling(totalCount / (double) pageSize);
    }
    
    public int TotalPage { get; }
    public int PageNumber { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPage;
}