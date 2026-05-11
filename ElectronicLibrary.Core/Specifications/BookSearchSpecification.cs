using ElectronicLibrary.Core.Entities;
using System;

namespace ElectronicLibrary.Core.Specifications;

/// <summary>
/// Специфікація для пошуку книг за ключовим словом і статусом доступності.
/// </summary>
public class BookSearchSpecification : BaseSpecification<Book>
{
    public BookSearchSpecification(string? searchTerm, bool onlyAvailable)
        : base(b =>
            (string.IsNullOrEmpty(searchTerm) || b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm)) &&
            (!onlyAvailable || b.IsAvailable))
    {
    }
}