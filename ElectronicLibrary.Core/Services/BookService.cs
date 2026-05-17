using ElectronicLibrary.Core.DTOs;
using ElectronicLibrary.Core.Entities;
using ElectronicLibrary.Core.Interfaces;
using ElectronicLibrary.Core.Models;
using ElectronicLibrary.Core.Specifications;
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

    public async Task<PaginatedList<Book>> GetBooksAsync(ISortStrategy sortStrategy, string? searchTerm = null, bool onlyAvailable = false, int pageNumber = 1, int pageSize = 5)
    {
        var searchSpec = new BookSearchSpecification(searchTerm, onlyAvailable);
        var books = await _unitOfWork.Books.FindAsync(searchSpec);

        var sortedBooks = sortStrategy.Sort(books).ToList();

        var count = sortedBooks.Count;
        var items = sortedBooks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<Book>(items, count, pageNumber, pageSize);
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

    public async Task<BookDto?> GetBookDtoByIdAsync(int id)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(id);
        if (book == null) return null;

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            PublicationYear = book.PublicationYear,
            Isbn = book.Isbn
        };
    }

    public async Task AddBookAsync(BookDto bookDto)
    {
        var book = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            PublicationYear = bookDto.PublicationYear,
            Isbn = bookDto.Isbn,
            IsAvailable = true 
        };

        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(BookDto bookDto)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(bookDto.Id);
        if (book == null) throw new ArgumentException("Книгу не знайдено.");

        book.Title = bookDto.Title;
        book.Author = bookDto.Author;
        book.PublicationYear = bookDto.PublicationYear;
        book.Isbn = bookDto.Isbn;

        await _unitOfWork.Books.UpdateAsync(book);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(id);
        if (book == null) throw new ArgumentException("Книгу не знайдено.");

        var loans = await _unitOfWork.Loans.GetAllAsync();
        if (loans.Any(l => l.BookId == id && l.ReturnDate == null))
            throw new InvalidOperationException("Неможливо видалити книгу, яка зараз видана читачу.");

        await _unitOfWork.Books.DeleteAsync(book);
        await _unitOfWork.SaveChangesAsync();
    }
}