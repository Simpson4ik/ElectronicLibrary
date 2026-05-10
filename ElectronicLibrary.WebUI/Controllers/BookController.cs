using ElectronicLibrary.Core.Interfaces;
using ElectronicLibrary.Core.Strategies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ElectronicLibrary.WebUI.Controllers;

/// <summary>
/// Контролер для обробки HTTP-запитів сторінки каталогу книг.
/// </summary>
// Principle: Dependency Injection - контролер не створює сервіс самостійно, а отримує його абстракцію через конструктор.
// Principle: Single Responsibility - клас відповідає лише за прийом запитів та повернення View.
public class BookController : Controller
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<IActionResult> Index(string sortOrder = "title")
    {
        ISortStrategy sortStrategy = sortOrder == "year"
            ? new YearSortStrategy()
            : new TitleSortStrategy();

        var books = await _bookService.GetBooksAsync(sortStrategy);

        ViewBag.CurrentSort = sortOrder;

        return View(books);
    }
    /// <summary>
    /// Обробляє POST-запит для видачі книги за її ідентифікатором.
    /// Використовує патерн Post-Redirect-Get для уникнення повторної відправки форми.
    /// </summary>
    // Principle: REST - використання POST-запиту для зміни стану ресурсу на сервері.
    // Refactoring Technique: Extract Method - логіка обробки помилок винесена на рівень UI, а сама бізнес-логіка залишається в сервісі.
    [HttpPost]
    public async Task<IActionResult> Borrow(int id)
    {
        try
        {
            await _bookService.BorrowBookAsync(id);
            TempData["SuccessMessage"] = "Книгу успішно видано читачу!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Обробляє POST-запит для повернення книги в бібліотеку.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Return(int id)
    {
        try
        {
            await _bookService.ReturnBookAsync(id);
            TempData["SuccessMessage"] = "Книгу успішно повернено до фонду бібліотеки!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}   