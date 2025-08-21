using System;
using System.Buffers.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Data.Contexts;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specifications;

namespace Store.Repository.Repositories;

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    private readonly StoreDbContext _context;
    public GenericRepository(StoreDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);

    public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);
    public async Task<IReadOnlyList<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();


    public async Task<TEntity> GetByIdAsync(TKey? id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity == null)
            throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} with id '{id}' was not found.");
        return entity;
    }
    public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
    public async Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specifications)
    {
        return await SpecificationEvaluater<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), specifications).ToListAsync();
    }

    public async Task<TEntity> GetWithSpecificationByIdAsync(ISpecification<TEntity> specifications)
    {
        return await SpecificationEvaluater<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), specifications).FirstOrDefaultAsync();
    }

    // public async Task<int> GetAllCountAsync()
    // {
    //     return await _context.Set<TEntity>().CountAsync();
    // }

    public async Task<int> GetCountWithSpecificationAsync(ISpecification<TEntity> specification)
    {
        return await SpecificationEvaluater<TEntity, TKey>.GetQuery(_context.Set<TEntity>(),specification).CountAsync();
    }
}
