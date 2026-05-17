using ElectronicLibrary.Core.Entities;
using ElectronicLibrary.Core.Strategies;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicLibrary.Core.DTOs;
using ElectronicLibrary.Core.Models;


namespace ElectronicLibrary.Core.Interfaces;

/// <summary>
/// Інтерфейс сервісу бізнес-логіки для операцій з книгами.
/// </summary>
public interface IBookService
{
    Task BorrowBookAsync(int bookId);

    Task<BookDto?> GetBookDtoByIdAsync(int id);
    Task AddBookAsync(BookDto bookDto);
    Task UpdateBookAsync(BookDto bookDto);
    Task DeleteBookAsync(int id);
    Task<PaginatedList<Book>> GetBooksAsync(ISortStrategy sortStrategy, string? searchTerm = null, bool onlyAvailable = false, int pageNumber = 1, int pageSize = 5);
    Task ReturnBookAsync(int bookId);
}