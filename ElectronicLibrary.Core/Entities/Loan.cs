using System;

namespace ElectronicLibrary.Core.Entities;

/// <summary>
/// Представляє запис про видачу книги читачу.
/// </summary>
// Principle: Single Responsibility - клас відповідає лише за дані про транзакцію видачі.
public class Loan
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int ReaderId { get; set; }
    public Reader Reader { get; set; } = null!;

    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14);

    public bool IsReturned => ReturnDate.HasValue;
}