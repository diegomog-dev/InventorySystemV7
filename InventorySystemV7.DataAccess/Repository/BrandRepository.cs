using InventorySystemV7.DataAccess.Data;
using InventorySystemV7.DataAccess.Repository.IRepository;
using InventorySystemV7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.DataAccess.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Brand brand)
        {
            var brandDB = _context.Brands.FirstOrDefault( w => w.Id == brand.Id);
            if (brandDB != null)
            {
                brandDB.Name = brand.Name;
                brandDB.Description = brand.Description;
                brandDB.State = brand.State;
                _context.SaveChanges();
            }
        }
    }
}
