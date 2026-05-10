using System.ComponentModel.DataAnnotations;

namespace ElectronicLibrary.Core.DTOs;

/// <summary>
/// Об'єкт передачі даних для створення та редагування книг.
/// </summary>
// Principle: Separation of Concerns - відділяємо модель бази даних від моделі представлення даних.
public class BookDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Назва книги є обов'язковою.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Назва має містити від 2 до 200 символів.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ім'я автора є обов'язковим.")]
    [StringLength(100, ErrorMessage = "Ім'я автора не може перевищувати 100 символів.")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Рік видання є обов'язковим.")]
    [Range(1400, 2100, ErrorMessage = "Будь ласка, введіть коректний рік видання.")]
    public int PublicationYear { get; set; }

    [Required(ErrorMessage = "ISBN є обов'язковим.")]
    [RegularExpression(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$", ErrorMessage = "Некоректний формат ISBN.")]
    public string Isbn { get; set; } = string.Empty;
}