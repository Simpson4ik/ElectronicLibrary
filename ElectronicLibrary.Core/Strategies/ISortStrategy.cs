using ElectronicLibrary.Core.Entities;
using System.Collections.Generic;

namespace ElectronicLibrary.Core.Strategies;

/// <summary>
/// Інтерфейс стратегії для різних алгоритмів сортування книг.
/// </summary>
// Design Pattern: Strategy - визначає сімейство алгоритмів сортування, роблячи їх взаємозамінними.
public interface ISortStrategy
{
    IEnumerable<Book> Sort(IEnumerable<Book> books);
}