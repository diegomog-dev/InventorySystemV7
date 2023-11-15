using InventorySystemV7.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystemV7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WineryController : Controller
    {        
        private readonly IUnitWork _unitWork;

        public WineryController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitWork.Winery.GetAll();
            return Json(new {data = all});
        }
        #endregion
    }
}
