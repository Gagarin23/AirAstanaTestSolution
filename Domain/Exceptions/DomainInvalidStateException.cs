using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Domain.Constants;
using FluentValidation.Results;

namespace Domain.Exceptions;

public class DomainInvalidStateException : BusinessValidationException
{
    public DomainInvalidStateException(string property, string message, bool appendDefaultMessage)
        : base(new []{new ValidationFailure(property, message)}, appendDefaultMessage) { }

    public DomainInvalidStateException(string property, string message)
        : base(new []{new ValidationFailure(property, message)}) { }
}
