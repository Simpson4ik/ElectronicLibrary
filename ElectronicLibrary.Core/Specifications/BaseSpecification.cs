using System;
using System.Linq.Expressions;

namespace ElectronicLibrary.Core.Specifications;

/// <summary>
/// Базовий клас специфікації.
/// </summary>
public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
}