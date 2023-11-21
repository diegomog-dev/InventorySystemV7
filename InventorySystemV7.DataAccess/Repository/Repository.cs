using InventorySystemV7.DataAccess.Data;
using InventorySystemV7.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        
        public Repository(ApplicationDbContext db)
        {
            _context = db;
            this.dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);   // insert into Table
        }

        public async Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);       // select * from (solo por Id)
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string addPropierties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);        // select * from where ...
            }
            if(addPropierties != null)
            {
                foreach(var item in addPropierties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);        // ejemplo "Categoria,Marca"
                }
            }
            if(orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetFirst(Expression<Func<T, bool>> filter = null, string addPropierties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);        // select * from where ...
            }
            if (addPropierties != null)
            {
                foreach (var item in addPropierties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);        // ejemplo "Categoria,Marca"
                }
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

    }
}
