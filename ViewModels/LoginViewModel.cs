using System.ComponentModel.DataAnnotations;

namespace Bicycle.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "작성이 필요한 필드입니다.")]
    public string UserName { get; set; }
    
    [Required]
    [DataType(DataType.Password, ErrorMessage = "작성이 필요한 필드입니다.")]
    [MinLength(6)]
    public string Password { get; set; }
}