using System;
using Store.Data.Entities;
using Store.Repository.Specifications;

namespace Store.Repository.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<TEntity> GetByIdAsync(TKey? id);
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<TEntity> GetWithSpecificationByIdAsync(ISpecification<TEntity> specifications);
    Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specifications);
    Task AddAsync(TEntity entity);
    // Task<int> GetAllCountAsync();
    Task<int> GetCountWithSpecificationAsync(ISpecification<TEntity> specification);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
