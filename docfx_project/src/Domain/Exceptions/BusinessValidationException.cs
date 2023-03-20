using System.Collections.Generic;
using Domain.Constants;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.Exceptions;

public class BusinessValidationException : ValidationException
{
    public BusinessValidationException(IEnumerable<ValidationFailure> errors, bool appendDefaultMessage)
        : base
        (
            ValidationMessages.ValidationFailure,
            errors,
            appendDefaultMessage
        ) { }

    public BusinessValidationException(IEnumerable<ValidationFailure> errors)
        : base(errors) { }
}
