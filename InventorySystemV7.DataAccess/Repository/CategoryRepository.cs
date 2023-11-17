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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var categoryDB = _context.Categories.FirstOrDefault( w => w.Id == category.Id);
            if (categoryDB != null)
            {
                categoryDB.Name = category.Name;
                categoryDB.Description = category.Description;
                categoryDB.State = category.State;
                _context.SaveChanges();
            }
        }
    }
}
