using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ordering.API.Endpoints
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
        public IEnumerable<string> ErrorMessages { get; set; }

        public ErrorResponse(string errorMessage)
            : this(new List<string> { errorMessage }) { }

        public ErrorResponse(IEnumerable<string> errorMessages)
        {
            this.ErrorMessages = errorMessages;
        }
    }
}
