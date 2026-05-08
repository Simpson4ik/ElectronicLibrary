using ElectronicLibrary.Core.Entities;
using ElectronicLibrary.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace ElectronicLibrary.Infrastructure.Data;

/// <summary>
/// Реалізація патерну Unit of Work для роботи з Entity Framework Core.
/// </summary>
// Design Pattern: Unit of Work.
// Principle: Single Responsibility - клас відповідає лише за координацію транзакцій.
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IRepository<Book>? _books;
    private IRepository<Reader>? _readers;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<Book> Books => _books ??= new Repository<Book>(_context);

    public IRepository<Reader> Readers => _readers ??= new Repository<Reader>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}