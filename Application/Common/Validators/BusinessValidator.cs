using System.Collections.Generic;
using FluentValidation;

namespace Application.Common.Validators;

/// <summary>
/// Валидатор бизнес логики
/// </summary>
/// <typeparam name="T"></typeparam>
public class BusinessValidator<T> : AbstractValidator<T> { }
