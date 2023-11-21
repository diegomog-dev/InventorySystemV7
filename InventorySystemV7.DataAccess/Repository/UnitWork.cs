using InventorySystemV7.DataAccess.Data;
using InventorySystemV7.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.DataAccess.Repository
{
    public class UnitWork : IUnitWork
    {
        private readonly ApplicationDbContext _context;
        public IWineryRepository Winery {  get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IProductRepository Product { get; private set; }

        public UnitWork(ApplicationDbContext context)
        {
            _context = context;
            Winery = new WineryRepository(context);
            Category = new CategoryRepository(context);
            Brand = new BrandRepository(context);
            Product = new ProductRepository(context);
        }

        public void Dispose()       // para liberar todos los repositorios o recursos no necesarios
        {
            _context.Dispose();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
