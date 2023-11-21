using InventorySystemV7.DataAccess.Repository.IRepository;
using InventorySystemV7.Models;
using InventorySystemV7.Models.ViewModels;
using InventorySystemV7.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystemV7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitWork _unitWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitWork unitWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitWork = unitWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitWork.Product.GetAllDropdownList("Category"),
                BrandList = _unitWork.Product.GetAllDropdownList("Brand"),
                FatherList = _unitWork.Product.GetAllDropdownList("Product")
            };

            if(id == null)
            {
                // Crete new Product
                productVM.Product.State = true;
                return View(productVM);
            }
            else
            {
                productVM.Product = await _unitWork.Product.GetById(id.GetValueOrDefault());
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    // Create new product
                    string upload = webRootPath + DS.ImageRoute;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    // Create new Image on Disk
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.UrlImage = fileName + extension;
                    await _unitWork.Product.Add(productVM.Product);
                }
                else
                {
                    // Update product
                    var objProduct = await _unitWork.Product.GetFirst(p => p.Id == productVM.Product.Id, isTracking:false);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + DS.ImageRoute;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        // Delete old image
                        var oldImage = Path.Combine(upload, objProduct.UrlImage);
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }

                        // Create new Image on Disk
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.UrlImage = fileName + extension;
                    } // In the event that a new image is not loaded and the existing one is left in place
                    else
                    {
                        productVM.Product.UrlImage = objProduct.UrlImage;
                    }
                    _unitWork.Product.Update(productVM.Product);
                }
                TempData[DS.Success] = "Guardado con éxito!";
                await _unitWork.Save();

                return View("Index");
            }
            // If not Valid
            productVM.CategoryList = _unitWork.Product.GetAllDropdownList("Category");
            productVM.CategoryList = _unitWork.Product.GetAllDropdownList("Brand");
            productVM.CategoryList = _unitWork.Product.GetAllDropdownList("Product");
            return View(productVM);
        }
                
        #region API
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitWork.Product.GetAll(addPropierties:"Category,Brand");
            return Json(new {data = all});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productDb = await _unitWork.Product.GetById(id);
            if(productDb == null)
            {
                return Json(new {success = false, message = "Error al borrar el Producto"});
            }

            // Delete image on disk
            string upload = _webHostEnvironment.WebRootPath + DS.ImageRoute;
            var oldFile = Path.Combine(upload, productDb.UrlImage);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _unitWork.Product.Delete(productDb);
            await _unitWork.Save();
            return Json(new { success = true, message = "Producto eliminada satisfactoriamente" });
        }

        [ActionName("SerialValidation")]
        public async Task<IActionResult> SerialValidation(string serial, int id=0)
        {
            bool value = false;
            var list = await _unitWork.Product.GetAll();
            if(id == 0)
            {
                value = list.Any(b => b.SerialNumber.ToLower().Trim() == serial.ToLower().Trim());
            }
            else
            {
                value = list.Any(b => b.SerialNumber.ToLower().Trim() == serial.ToLower().Trim() && b.Id != id);
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
