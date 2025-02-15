using System.ComponentModel.DataAnnotations;

namespace Bicycle.Models;

public class Review
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string? Name { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string? Title { get; set; }
    
    public string? Contents { get; set; }
    
    [MaxLength(6)]
    [Required]
    public string? Number { get; set; }
    
    [Required]
    public DateTime Date { get; set; } = DateTime.Now;
    
    [Range(1, 10)]
    [Required]
    public int Rating { get; set; }
    
    public int FileId { get; set; }
    
    public FileModel File { get; set; } = new FileModel();
}