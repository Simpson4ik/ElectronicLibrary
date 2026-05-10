using ElectronicLibrary.Core.Entities;
using System;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.Interfaces;

// Design Pattern: Unit of Work - об'єднує декілька операцій з репозиторіями в одну логічну транзакцію.
// Principle: Interface Segregation - клієнти не залежать від реалізації бази даних.
public interface IUnitOfWork : IDisposable
{
    IRepository<Book> Books { get; }
    IRepository<Reader> Readers { get; }
    Task<int> SaveChangesAsync();
    IRepository<Loan> Loans { get; }
}