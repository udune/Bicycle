using System.ComponentModel.DataAnnotations;

namespace Bicycle.Models;

public class Review
{
    [Key]
    public int Id { get; set; }
    
    public int GroupNum { get; set; }
    public int GroupOrder { get; set; }
    public int ParentId { get; set; }
    public int GroupTap { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string? Name { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string? Title { get; set; }
    
    public string? Contents { get; set; }

    [MaxLength(5)] 
    [Required] 
    public string? Number { get; set; } = "12345";
    
    [Required]
    public DateTime Date { get; set; } = DateTime.Now;

    [Range(1, 10)] 
    [Required] 
    public int Rating { get; set; } = 8;
    
    public int FileId { get; set; }
    
    public FileModel File { get; set; } = new FileModel();
}