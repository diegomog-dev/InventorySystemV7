using InventorySystemV7.DataAccess.Repository.IRepository;
using InventorySystemV7.Models;
using InventorySystemV7.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystemV7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitWork _unitWork;

        public CategoryController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                // Create new Winery
                category.State = true;
                return View(category);
            }
            else
            {
                // Update the exist winery
                category = await _unitWork.Category.GetById(id.GetValueOrDefault());
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category category)
        {
            if(ModelState.IsValid)
            {
                if(category.Id == 0)
                {
                    await _unitWork.Category.Add(category);
                    TempData[DS.Success] = "Categoria creada exitosamente.";
                }
                else
                {
                    _unitWork.Category.Update(category);
                    TempData[DS.Success] = "Categoria actualizada exitosamente.";
                }

                await _unitWork.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar la Categoria.";
            return View(category);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitWork.Category.GetAll();
            return Json(new {data = all});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryDb = await _unitWork.Category.GetById(id);
            if(categoryDb == null)
            {
                return Json(new {success = false, message = "Error al borrar la Categoria"});
            }
            _unitWork.Category.Delete(categoryDb);
            await _unitWork.Save();
            return Json(new { success = true, message = "Categoria eliminada satisfactoriamente" });
        }

        [ActionName("NameValidation")]
        public async Task<IActionResult> NameValidation(string name, int id=0)
        {
            bool value = false;
            var list = await _unitWork.Category.GetAll();
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
