using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.Shared
{
    public static class GetModelErrorMessages
    {
        public static ErrorResponse BadRequestModelState(ModelStateDictionary modelState)
        {
            var errorMessage = modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return new ErrorResponse(errorMessage);
        }
    }

    public record ErrorResponse
    {
        public IEnumerable<string> ErrorMessages { get; }

        public ErrorResponse(string errorMessage)
            : this(new List<string> { errorMessage }) { }

        [JsonConstructor]
        public ErrorResponse(IEnumerable<string> errorMessages)
        {
            this.ErrorMessages = errorMessages;
        }
    }
}
