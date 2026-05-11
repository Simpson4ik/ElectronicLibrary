using ElectronicLibrary.Core.Entities;
using ElectronicLibrary.Core.Strategies;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicLibrary.Core.DTOs;

namespace ElectronicLibrary.Core.Interfaces;

/// <summary>
/// Інтерфейс сервісу бізнес-логіки для операцій з книгами.
/// </summary>
public interface IBookService
{
    Task<IEnumerable<Book>> GetBooksAsync(ISortStrategy sortStrategy);
    Task BorrowBookAsync(int bookId);

    Task<BookDto?> GetBookDtoByIdAsync(int id);
    Task AddBookAsync(BookDto bookDto);
    Task UpdateBookAsync(BookDto bookDto);
    Task DeleteBookAsync(int id);

    /// <summary>
    /// Реєструє повернення книги в бібліотеку, закриваючи активний запис про видачу.
    /// </summary>
    Task ReturnBookAsync(int bookId);
}