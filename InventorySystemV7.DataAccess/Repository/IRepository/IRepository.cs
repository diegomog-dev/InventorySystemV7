using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id);      // Devuelve objeto T asociado al id que se envía como parametro
        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>> filter = null,                    // para poder filtrar la lista
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,   // para ordenar la lista
            string addPropierties = null,                               // incluir propiedades adicionales al id
            bool isTracking = true                                      // acceder al objeto y porderlo modificar
            );
        Task<T> GetFirst(
            Expression<Func<T, bool>> filter = null,
            string addPropierties = null,
            bool isTracking = true
            );
        Task Add( T entity );
        void Delete( T entity );
        void DeleteRange(IEnumerable<T> entities );
    }
}
