using System.ComponentModel.DataAnnotations;

namespace Bicycle.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "작성이 필요한 필드입니다.")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "작성이 필요한 필드입니다.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "이메일 형식이 아닙니다.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "작성이 필요한 필드입니다.")]
    [DataType(DataType.Password, ErrorMessage = "비밀번호 형식이 아닙니다.")]
    [MinLength(6, ErrorMessage = "비밀번호는 6자리 이상이어야 합니다.")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "작성이 필요한 필드입니다.")]
    [DataType(DataType.Password, ErrorMessage = "비밀번호 형식이 아닙니다.")]
    [Compare("Password", ErrorMessage = "비밀번호가 서로 일치하지 않습니다.")]
    [MinLength(6, ErrorMessage = "비밀번호는 6자리 이상이어야 합니다.")]
    public string ConfirmPassword { get; set; }
}