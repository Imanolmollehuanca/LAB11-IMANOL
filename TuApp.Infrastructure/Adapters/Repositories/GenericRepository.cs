using Microsoft.EntityFrameworkCore;
using TuApp.Domain.Repositories;
using TuApp.Infrastructure.Context;

namespace TuApp.Infrastructure.Adapters.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    private readonly TicketeraContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(TicketeraContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            return null;

        return await _dbSet.FindAsync(guid);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is null)
            return;

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}