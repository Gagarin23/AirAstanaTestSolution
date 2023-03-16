using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Common.Validators;

public abstract partial class InputValidator<T> : AbstractValidator<T>
{
    protected bool ValidateId(long id)
    {
        return id > 0;
    }
}
