using System.ComponentModel.DataAnnotations.Schema;

namespace Bicycle.Models;

public class FileModel
{
    public int Id { get; set; }
    public string? OriginalName { get; set; }
    public string? OriginalType { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
}