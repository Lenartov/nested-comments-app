using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace NestedComments.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ValidEmailAddressAttribute : ValidationAttribute
    {
        public ValidEmailAddressAttribute(string? errorMessage = null)
        {
            ErrorMessage = errorMessage ?? "Invalid email";
        }

        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))      
                return true;
            

            if (value is not string email || string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new MailAddress(email);
                var domain = addr.Host;
                int dotIndex = domain.LastIndexOf('.');

                if (dotIndex == -1)
                    return false;
                

                if (dotIndex == domain.Length - 1)
                    return false;
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
