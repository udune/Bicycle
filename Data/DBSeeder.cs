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
        if (!_context.Teachers.Any())
        {
            List<Teacher> teachers = new List<Teacher>()
            {
                new Teacher() { Name = "세종대왕", Class = "한글" },
                new Teacher() { Name = "이순신", Class = "해상전략" },
                new Teacher() { Name = "제갈량", Class = "지략" },
                new Teacher() { Name = "을지문덕", Class = "지상전략" },
            };

            await _context.AddRangeAsync(teachers);
            await _context.SaveChangesAsync();
        }
    }
}