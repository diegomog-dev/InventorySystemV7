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
    public class WineryRepository : Repository<Winery>, IWineryRepository
    {
        private readonly ApplicationDbContext _context;

        public WineryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Winery winery)
        {
            var wineryDB = _context.Wineries.FirstOrDefault( w => w.Id == winery.Id);
            if (wineryDB != null)
            {
                wineryDB.Name = winery.Name;
                wineryDB.Description = winery.Description;
                wineryDB.State = winery.State;
                _context.SaveChanges();
            }
        }
    }
}
