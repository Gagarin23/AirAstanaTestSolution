using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Common.Validators;

//partial, т.е. в .net7 используем автосгенерированнные регулярки. Удалил т.к. в .net6 не поддерживается.
public abstract partial class InputValidator<T> : AbstractValidator<T>
{
    protected bool ValidateId(long id)
    {
        return id > 0;
    }
}
