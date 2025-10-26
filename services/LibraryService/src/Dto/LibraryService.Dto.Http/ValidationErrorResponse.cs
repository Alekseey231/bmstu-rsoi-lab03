using System.Runtime.Serialization;

namespace LibraryService.Dto.Http;

public class ValidationErrorResponse
{
    [DataMember(Name = "message")]
    public string? Message { get; set; }
    
    [DataMember(Name = "errors")]
    public Dictionary<string, string>? Errors { get; set; }

    public ValidationErrorResponse(string? message, Dictionary<string, string>? errors)
    {
        Message = message;
        Errors = errors;
    }
}