using System.ComponentModel.DataAnnotations;

namespace Bicycle.Models;

public class Review
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string? Name { get; set; }
    
    [MaxLength(100)]
    public string? Title { get; set; }
    
    [MaxLength(6)]
    public string? Number { get; set; }
    
    public DateTime Date { get; set; } = DateTime.Now;
    
    [Range(1, 10)]
    public int Rating { get; set; }
}