using Botticelli.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.DataLayer;

/// <summary>
///     Database storage
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
/// <typeparam name="TId">ID type</typeparam>
public class DbStorage<T, TId> : IStorage<T, TId>
        where T : class
{
    private readonly DbContext _context;

    public DbStorage(DbContext context)
    {
        _context = context;
    }

    public T? Get(TId id)
    {
        return _context.Find<T>(id);
    }

    public ICollection<T>? Get(Func<T, bool> filter)
    {
        return _context.Set<T>()?.Where(filter).ToList();
    }

    public void Add(params T[] entites)
    {
        _context.AddRange(entites);
    }

    public void Remove(TId id)
    {
        var entity = Get(id);

        if (entity == default) return;

        Remove(entity);
    }

    public void Remove(T entity)
    {
        _context.Remove(entity);
    }

    public void Update(params T[] entities)
    {
        _context.Update(entities);
    }
}