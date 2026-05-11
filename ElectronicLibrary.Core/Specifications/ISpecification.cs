using System;
using System.Linq.Expressions;

namespace ElectronicLibrary.Core.Specifications;

/// <summary>
/// Інтерфейс патерну Specification.
/// Визначає фільтр для пошуку сутностей у базі даних.
/// </summary>
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
}