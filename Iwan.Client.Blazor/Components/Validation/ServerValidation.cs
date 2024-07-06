using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;

namespace Iwan.Client.Blazor.Components.Validation
{
    public class ServerValidation : ComponentBase
    {
        private ValidationMessageStore _messageStore;

        [CascadingParameter] EditContext CurrentEditContext { get; set; }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(ServerValidation)} requires a cascading parameter " +
                    $"of type {nameof(EditContext)}. For example, you can use {nameof(ServerValidation)} inside " +
                    $"an {nameof(EditForm)}.");
            }

            _messageStore = new ValidationMessageStore(CurrentEditContext);
            CurrentEditContext.OnValidationRequested += (s, e) => _messageStore.Clear();
            CurrentEditContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);
        }

        public void DisplayErrors(Dictionary<string, List<string>> errors)
        {
            foreach (var err in errors)
            {
                _messageStore.Add(CurrentEditContext.Field(err.Key), err.Value);
            }
            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void DisplayErrors(IEnumerable<(object model, string relatedProperty, string errorMessage)> errors)
        {
            foreach ((var model, var relatedProperty, var errorMessage) in errors)
            {
                var fieldIdentifier = new FieldIdentifier(model, relatedProperty);
                _messageStore.Clear(fieldIdentifier);
                _messageStore.Add(fieldIdentifier, errorMessage);
            }
            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void DisplayError(string field, string validationMessage)
        {
            var dictionary = new Dictionary<string, List<string>>
            {
                { field, new List<string> { validationMessage } }
            };

            DisplayErrors(dictionary);
        }
    }
}
