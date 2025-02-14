using Bicycle.Models;

namespace Bicycle.Data.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly ApplicationDbContext _context;

    public TeacherRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Teacher> GetAllTeachers()
    {
        var result = _context.Teachers.ToList();
        return result;
    }

    public Teacher GetTeacher(int id)
    {
        var result = _context.Teachers.Find(id);
        return result;
    }
}