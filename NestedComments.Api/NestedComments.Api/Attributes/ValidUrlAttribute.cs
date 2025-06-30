using AngleSharp.Dom;
using System.ComponentModel.DataAnnotations;

namespace NestedComments.Api.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ValidUrlAttribute : ValidationAttribute
    {
        public ValidUrlAttribute()
        {
            ErrorMessage = "Поле має містити валідну URL-адресу.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            var url = value as string;

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
            {
                return false;
            }

            if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            return true;
        }
    }
}
