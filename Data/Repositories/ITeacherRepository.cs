using Bicycle.Models;

namespace Bicycle.Data.Repositories;

public interface ITeacherRepository
{
    IEnumerable<Teacher> GetAllTeachers();
    Teacher GetTeacher(int id);
}