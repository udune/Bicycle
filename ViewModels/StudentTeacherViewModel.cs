using Bicycle.Models;

namespace Bicycle.ViewModels;

public class StudentTeacherViewModel
{
    public Student Student { get; set; } = new Student();
    public IEnumerable<Teacher> Teachers { get; set; } = new List<Teacher>();
    public IEnumerable<Student> Students { get; set; } = new List<Student>();
}