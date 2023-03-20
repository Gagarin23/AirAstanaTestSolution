using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.Exceptions;

public class InputValidationException : ValidationException
{
    public InputValidationException(string message)
        : base(message) { }

    public InputValidationException(string message, IEnumerable<ValidationFailure> errors)
        : base(message, errors) { }

    public InputValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage)
        : base
        (
            message,
            errors,
            appendDefaultMessage
        ) { }

    public InputValidationException(IEnumerable<ValidationFailure> errors)
        : base(errors) { }
}
