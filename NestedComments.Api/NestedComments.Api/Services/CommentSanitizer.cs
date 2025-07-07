using Ganss.Xss;
using NestedComments.Api.Services.Interfaces;

namespace NestedComments.Api.Services
{

    public class CommentSanitizer : ICommentSanitizer
    {
        private readonly HtmlSanitizer _sanitizer;

        public CommentSanitizer()
        {
            _sanitizer = new HtmlSanitizer();

            _sanitizer.AllowedTags.Clear();
            _sanitizer.AllowedTags.Add("a");
            _sanitizer.AllowedTags.Add("code");
            _sanitizer.AllowedTags.Add("i");
            _sanitizer.AllowedTags.Add("strong");

            _sanitizer.AllowedAttributes.Clear();
            _sanitizer.AllowedAttributes.Add("href");
            _sanitizer.AllowedAttributes.Add("title");

            _sanitizer.AllowedSchemes.Clear();
            _sanitizer.AllowedSchemes.Add("http");
            _sanitizer.AllowedSchemes.Add("https");
        }

        public string Sanitize(string input)
        {
            return _sanitizer.Sanitize(input);
        }

        public bool IsContainValidTags(string input)
        {
            var sanitaizedInput = _sanitizer.Sanitize(input);

            if (sanitaizedInput == input)
            {
                return true;
            }

            return false;
        }
    }
}