using ElectronicLibrary.Core.Entities;
using ElectronicLibrary.Core.Interfaces;
using ElectronicLibrary.Core.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
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

        if (book == null) throw new ArgumentException("Книгу не знайдено.");
        if (!book.IsAvailable) throw new InvalidOperationException("Книга вже видана.");

        var loan = new Loan
        {
            BookId = book.Id,
            ReaderId = 1,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14)
        };

        book.IsAvailable = false;

        await _unitOfWork.Loans.AddAsync(loan);
        await _unitOfWork.Books.UpdateAsync(book);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Логіка повернення книги: оновлення статусу книги та закриття запису про видачу.
    /// </summary>
    // Principle: Atomicity - завдяки Unit of Work обидві операції (оновлення книги та логу) виконаються як одна транзакція.
    public async Task ReturnBookAsync(int bookId)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(bookId);
        if (book == null) throw new ArgumentException("Книгу не знайдено.");

        var loans = await _unitOfWork.Loans.GetAllAsync();
        var activeLoan = loans.FirstOrDefault(l => l.BookId == bookId && l.ReturnDate == null);

        if (activeLoan == null)
            throw new InvalidOperationException("Ця книга зараз не рахується як видана.");

        activeLoan.ReturnDate = DateTime.UtcNow;

        book.IsAvailable = true;

        await _unitOfWork.Loans.UpdateAsync(activeLoan);
        await _unitOfWork.Books.UpdateAsync(book);

        await _unitOfWork.SaveChangesAsync();
    }
}