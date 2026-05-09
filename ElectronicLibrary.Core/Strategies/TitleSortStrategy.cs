using ElectronicLibrary.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicLibrary.Core.Strategies;

/// <summary>
/// Стратегія сортування каталогу книг за алфавітом.
/// </summary>
public class TitleSortStrategy : ISortStrategy
{
    public IEnumerable<Book> Sort(IEnumerable<Book> books)
    {
        return books.OrderBy(b => b.Title);
    }
}