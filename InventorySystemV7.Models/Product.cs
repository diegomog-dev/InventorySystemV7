using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Número de serie es requerido")]
        [MaxLength(60)]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "Descripción es requerido")]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Precio es requerido")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Costo es requerido")]
        public double Cost { get; set; }
        public string UrlImage { get; set; }
        [Required(ErrorMessage = "Estado es requerido")]
        public bool State { get; set; }

        [Required(ErrorMessage = "Categoría es requerido")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Marca es requerido")]
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }

        public int? FatherId { get; set; }
        public virtual Product Father { get; set; }
    }
}
