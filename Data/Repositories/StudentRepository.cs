using Bicycle.Models;

namespace Bicycle.Data.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;
    
    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddStudent(Student student)
    {
        _context.Students.Add(student);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public IEnumerable<Student> GetAllStudents()
    {
        var result = _context.Students.ToList();
        return result;
    }

    public Student GetStudent(int id)
    {
        var result = _context.Students.Find(id);
        return result;
    }

    public void Edit(Student student)
    {
        _context.Update(student);
    }

    public void Delete(Student student)
    {
        _context.Remove(student);
    }
}