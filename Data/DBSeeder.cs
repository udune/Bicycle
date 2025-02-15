using Bicycle.Models;

namespace Bicycle.Data;

public class DBSeeder
{
    private ApplicationDbContext _context;
    
    public DBSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedDatabase()
    {
        if (!_context.Reviews.Any())
        {
            await _context.AddRangeAsync();
            await _context.SaveChangesAsync();
        }
    }
}