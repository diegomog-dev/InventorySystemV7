using InventorySystemV7.DataAccess.Repository.IRepository;
using InventorySystemV7.Models;
using InventorySystemV7.Utilities;
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

        public async Task<IActionResult> Upsert(int? id)
        {
            Winery winery = new Winery();
            if (id == null)
            {
                // Create new Winery
                winery.State = true;
                return View(winery);
            }
            else
            {
                // Update the exist winery
                winery = await _unitWork.Winery.GetById(id.GetValueOrDefault());
                if (winery == null)
                {
                    return NotFound();
                }

                return View(winery);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Winery winery)
        {
            if(ModelState.IsValid)
            {
                if(winery.Id == 0)
                {
                    await _unitWork.Winery.Add(winery);
                    TempData[DS.Success] = "Bodega creada exitosamente.";
                }
                else
                {
                    _unitWork.Winery.Update(winery);
                    TempData[DS.Success] = "Bodega actualizada exitosamente.";
                }

                await _unitWork.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar la Bodega.";
            return View(winery);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitWork.Winery.GetAll();
            return Json(new {data = all});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var wineryDb = await _unitWork.Winery.GetById(id);
            if(wineryDb == null)
            {
                return Json(new {success = false, message = "Error al borrar la Bodega"});
            }
            _unitWork.Winery.Delete(wineryDb);
            await _unitWork.Save();
            return Json(new { success = true, message = "Bodega eliminada satisfactoriamente" });
        }

        [ActionName("NameValidation")]
        public async Task<IActionResult> NameValidation(string name, int id=0)
        {
            bool value = false;
            var list = await _unitWork.Winery.GetAll();
            if(id == 0)
            {
                value = list.Any(b => b.Name.ToLower().Trim() == name.ToLower().Trim());
            }
            else
            {
                value = list.Any(b => b.Name.ToLower().Trim() == name.ToLower().Trim() && b.Id != id);
            }
            if (value)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }
        #endregion
    }
}
