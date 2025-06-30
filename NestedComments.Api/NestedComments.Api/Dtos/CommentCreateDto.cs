using NestedComments.Api.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NestedComments.Api.Attributes;

namespace NestedComments.Api.Dtos;

public class CommentCreateDto
{
    [Required(ErrorMessage = "User Name is required")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "User Name must contain only Latin letters and digits")]
    [StringLength(ValidationRules.UserNameMaxLength, ErrorMessage = "Username is too long")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [ValidEmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(ValidationRules.EmailMaxLength, ErrorMessage = "Email is too long")]
    public required string Email { get; set; }

    [JsonIgnore]
    [ValidUrl(ErrorMessage = "Invalid URL format")]
    public string? HomePage { get; set; }

    [Required(ErrorMessage = "Captcha is required")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Captcha must contain only Latin letters and digits")]
    public required string Captcha { get; set; }

    [Required(ErrorMessage = "Message text is required")]
    //[RegularExpression(@"\S+", ErrorMessage = "Message cannot contain only spaces")]
    [StringLength(ValidationRules.MessageMaxLength, ErrorMessage = "Message is too long")]
    public required string Message { get; set; }

    public int? ParentCommentId { get; set; }
}
