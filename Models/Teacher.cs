using System.ComponentModel.DataAnnotations;

namespace Bicycle.Models;

public class Teacher
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
}