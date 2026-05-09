using ElectronicLibrary.Core.Entities;
using ElectronicLibrary.Core.Interfaces;
using ElectronicLibrary.Core.Strategies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.Services;

/// <summary>
/// Реалізація бізнес-логіки для роботи з каталогом бібліотеки.
/// </summary>
// Principle: Open/Closed (SOLID) - клас закритий для модифікації, але відкритий для розширення.
public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(ISortStrategy sortStrategy)
    {
        var books = await _unitOfWork.Books.GetAllAsync();
        return sortStrategy.Sort(books);
    }

    public async Task BorrowBookAsync(int bookId)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(bookId);

        if (book == null)
            throw new ArgumentException("Книгу не знайдено в системі.");

        if (!book.IsAvailable)
            throw new InvalidOperationException("Ця книга вже видана іншому читачу.");

        book.IsAvailable = false;

        await _unitOfWork.Books.UpdateAsync(book);
        await _unitOfWork.SaveChangesAsync();
    }
}