using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using BLL.Interfaces;
using DAL.data.Database;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal readonly AmazonDbContext context;
        internal readonly DbSet<TEntity> dbSet;
        private readonly ISpecificationEvaluator _specificationEvaluator;

        public Repository(AmazonDbContext context, ISpecificationEvaluator specificationEvaluator)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
            this._specificationEvaluator = specificationEvaluator;
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return dbSet.AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(object id)
        {
            TEntity? entityToDelete = await dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                DeleteAsync(entityToDelete);
            }
        }

        public Task DeleteAsync(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<TEntity?> GetItemBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return _specificationEvaluator.GetQuery(dbSet, specification);
        }
    }
}

