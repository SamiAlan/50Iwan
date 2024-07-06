using Iwan.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Iwan.Client.Blazor.Extensions
{
    public static class GeneralExtensions
    {
        public static IEnumerable<(object, string, string)> ToProperValidationObject(this Dictionary<string, List<string>> errors, object rootRelatedObject)
        {
            var validationErrors = new List<(object, string, string)>();

            foreach (var error in errors)
            {
                (var relatedObject, var relatedProperty) = rootRelatedObject.GetPropertyValueAndRelatedObject(error.Key);
                validationErrors.Add((relatedObject, relatedProperty, error.Value.First()));
            }

            return validationErrors;
        }
    }
}
