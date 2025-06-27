using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

public class CommentCreateDto
{
    [Required(ErrorMessage = "User Name is required")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "User Name must contain only Latin letters and digits")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [JsonIgnore]
    [Url(ErrorMessage = "Invalid URL format")]
    public string? HomePage { get; set; }

    [Required(ErrorMessage = "Captcha is required")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Captcha must contain only Latin letters and digits")]
    public required string Captcha { get; set; }

    [Required(ErrorMessage = "Message text is required")]
    [StringLength(1000, ErrorMessage = "Message is too long")]
    public required string Message { get; set; }

    public int? ParentCommentId { get; set; }
}
