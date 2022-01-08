using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contrats;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(StoreContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }        

        public async Task<T> GetByIdAsync(int id) => await _entity.FindAsync(id);

        public async Task<IReadOnlyList<T>> ListAllAsync() => await _entity.ToListAsync();

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec) => await ApplySpecification(spec).ToListAsync();
        
        public async Task<T> GetEntityWithSpec(ISpecification<T> spec) => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecification<T> spec) => await ApplySpecification(spec).CountAsync();
       
        private IQueryable<T> ApplySpecification(ISpecification<T> spec) => SpecificationEvaluator<T>.GetQuery(_entity.AsQueryable(), spec);

       
    }
}
