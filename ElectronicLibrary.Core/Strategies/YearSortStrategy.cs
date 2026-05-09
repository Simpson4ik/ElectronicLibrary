using ElectronicLibrary.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicLibrary.Core.Strategies;

/// <summary>
/// Стратегія сортування каталогу книг за роком видання.
/// </summary>
public class YearSortStrategy : ISortStrategy
{
    public IEnumerable<Book> Sort(IEnumerable<Book> books)
    {
        return books.OrderByDescending(b => b.PublicationYear);
    }
}