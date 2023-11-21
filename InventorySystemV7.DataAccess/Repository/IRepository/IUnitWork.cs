using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.DataAccess.Repository.IRepository
{
    public interface IUnitWork : IDisposable
    {
        IWineryRepository Winery {  get; }
        ICategoryRepository Category { get; }
        IBrandRepository Brand { get; }
        IProductRepository Product { get; }
        Task Save();
    }
}
