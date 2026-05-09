using ElectronicLibrary.Core.Entities;
using ElectronicLibrary.Core.Strategies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.Interfaces;

/// <summary>
/// Інтерфейс сервісу бізнес-логіки для операцій з книгами.
/// </summary>
public interface IBookService
{
    Task<IEnumerable<Book>> GetBooksAsync(ISortStrategy sortStrategy);
    Task BorrowBookAsync(int bookId);
}