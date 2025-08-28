using System.Linq.Expressions;
using EFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Repo;

public class BaseRepository<TEntity>:IRepository<TEntity> where TEntity : class
{
    private readonly MovieDbContext dbContext;

    public BaseRepository(MovieDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        // throw new HttpRequestException("Simulated HTTP request failure.");
        
        await dbContext.Set<TEntity>().AddAsync(entity);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity, int id)
    {
        var returnedEntity=await GetByIdAsync(id);

        if (returnedEntity is null)
        {
            throw new KeyNotFoundException();
        }

        dbContext.Entry(returnedEntity).State = EntityState.Detached;
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync();

    }

    public async Task DeleteAsync(int id)
    {
        var returnedEntity = await GetByIdAsync(id);
        
        if (returnedEntity is null)
        {
            throw new KeyNotFoundException();
        }

        dbContext.Set<TEntity>().Remove(returnedEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var returnedEntity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        
        return returnedEntity is not null ? returnedEntity : null!;
    }

    public async Task<IEnumerable<TEntity>> GetMultipleByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var list = await dbContext.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync();
        
        return list is not null ? list : null!;
        
    }
}