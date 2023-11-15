using InventorySystemV7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.DataAccess.Repository.IRepository
{
    public interface IWineryRepository : IRepository<Winery>
    {
        void Update(Winery winery);
    }
}
