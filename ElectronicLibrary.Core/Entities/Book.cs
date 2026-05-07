using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicLibrary.Core.Entities;
/// <summary>
/// Представляє сутність книги в системі електронної бібліотеки.
/// </summary>
// Principle: Single Responsibility (SOLID) - Клас відповідає виключно за структуру даних книги.
public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Isbn { get; set; } = string.Empty;

    public int PublicationYear { get; set; }

    public bool IsAvailable { get; set; } = true;

    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
}

