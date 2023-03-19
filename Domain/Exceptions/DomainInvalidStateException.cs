using System;
using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class DomainInvalidStateException : Exception
{
    public DomainInvalidStateException() { }

    protected DomainInvalidStateException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public DomainInvalidStateException(string message)
        : base(message) { }

    public DomainInvalidStateException(string message, Exception innerException)
        : base(message, innerException) { }
}
