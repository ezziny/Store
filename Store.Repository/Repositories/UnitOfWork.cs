using System;
using System.Collections;
using System.Dynamic;
using Store.Data.Contexts;
using Store.Data.Entities;
using Store.Repository.Interfaces;

namespace Store.Repository.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable _repositories;
    private readonly StoreDbContext _context;
    public UnitOfWork(StoreDbContext context)
    {
        _context = context;
    }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        if (_repositories is null)
            _repositories = new Hashtable();
        var entityKey = typeof(TEntity).Name; // خلي الكي عبارة عن اسم الانتيتي
        if (!_repositories.ContainsKey(entityKey))
        {
            var repositoryType = typeof(GenericRepository<,>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TKey)), _context);
            _repositories.Add(entityKey, repositoryInstance);
        }
        return _repositories[entityKey] as IGenericRepository<TEntity, TKey>;
    }
}
