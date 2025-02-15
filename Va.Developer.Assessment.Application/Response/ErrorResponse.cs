using System.ComponentModel;

namespace Va.Developer.Assessment.Application.Response
{
    public class ErrorResponse(string message = "", bool succeeded = false, IEnumerable<string> errors = null) : IResponse
    {
        [DefaultValue("")]
        public string Message { get; set; } = message;
        [DefaultValue(true)]
        public bool Succeeded { get; set; } = succeeded;
        public IEnumerable<string> Errors { get; set; } = errors ?? [];
    }
}