using Botticelli.Auth.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Botticelli.Auth.Services;

public abstract class Manager<TEntityInfo, TEntity>(AuthDefaultDbContext authDefaultDbContext)
        : IManager<TEntityInfo> where TEntity : class
{
    private readonly DbSet<TEntity> _entities = authDefaultDbContext.Set<TEntity>();

    public virtual Task<IEnumerable<TEntityInfo>> Get(int from = 0, int pageSize = 20) 
        => Task.FromResult(_entities.Skip(from)
                                   .Take(pageSize)
                                   .ToList()
                                   .Adapt<IEnumerable<TEntityInfo>>());

    public virtual async Task Add(TEntityInfo info)
    {
        var adapted = info.Adapt<TEntity>();
        _entities.Add(adapted);

        await authDefaultDbContext.SaveChangesAsync();
    }

    public virtual async Task Update(TEntityInfo info)
    {
        _entities.Update(info.Adapt<TEntity>());

        await authDefaultDbContext.SaveChangesAsync();
    }
}