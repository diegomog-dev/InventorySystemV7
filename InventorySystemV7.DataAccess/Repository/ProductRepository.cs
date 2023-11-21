using InventorySystemV7.DataAccess.Data;
using InventorySystemV7.DataAccess.Repository.IRepository;
using InventorySystemV7.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if(obj == "Category")
            {
                return _context.Categories.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }

            if (obj == "Brand")
            {
                return _context.Brands.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }

            if (obj == "Product")
            {
                return _context.Products.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Description,
                    Value = c.Id.ToString()
                });
            }

            return null;
        }

        public void Update(Product product)
        {
            var productDB = _context.Products.FirstOrDefault( w => w.Id == product.Id);
            if (productDB != null)
            {
                // Validar si es necesario actualizar la imagen
                if(product.UrlImage != null)
                {
                    productDB.UrlImage = product.UrlImage;
                }
                productDB.SerialNumber = product.SerialNumber;
                productDB.Description = product.Description;
                productDB.Price = product.Price;
                productDB.Cost = product.Cost;
                productDB.CategoryId = product.CategoryId;
                productDB.BrandId = product.BrandId;
                productDB.FatherId = product.FatherId;
                productDB.State = product.State;
                _context.SaveChanges();
            }
        }
    }
}
