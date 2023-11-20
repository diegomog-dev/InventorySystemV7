using InventorySystemV7.DataAccess.Repository.IRepository;
using InventorySystemV7.Models;
using InventorySystemV7.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystemV7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitWork _unitWork;

        public BrandController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Brand brand = new Brand();
            if (id == null)
            {
                // Create new Brand
                brand.State = true;
                return View(brand);
            }
            else
            {
                // Update the exist Brand
                brand = await _unitWork.Brand.GetById(id.GetValueOrDefault());
                if (brand == null)
                {
                    return NotFound();
                }

                return View(brand);
            }
        }

        /* Insert and update */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Brand brand)
        {
            if(ModelState.IsValid)
            {
                if(brand.Id == 0)
                {
                    await _unitWork.Brand.Add(brand);
                    TempData[DS.Success] = "Marca creada exitosamente.";
                }
                else
                {
                    _unitWork.Brand.Update(brand);
                    TempData[DS.Success] = "Marca actualizada exitosamente.";
                }

                await _unitWork.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar la Marca.";
            return View(brand);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitWork.Brand.GetAll();
            return Json(new {data = all});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var brandDb = await _unitWork.Brand.GetById(id);
            if(brandDb == null)
            {
                return Json(new {success = false, message = "Error al borrar la Marca"});
            }
            _unitWork.Brand.Delete(brandDb);
            await _unitWork.Save();
            return Json(new { success = true, message = "Marca eliminada satisfactoriamente" });
        }

        [ActionName("NameValidation")]
        public async Task<IActionResult> NameValidation(string name, int id=0)
        {
            bool value = false;
            var list = await _unitWork.Brand.GetAll();
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
