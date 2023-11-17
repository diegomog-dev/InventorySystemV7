using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es requerido")]
        [MaxLength(60, ErrorMessage ="El nombre para la categoría debe ser máximo de 60 caracters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripción es requerido")]
        [MaxLength(100, ErrorMessage = "La descripción para la categoría debe ser máximo de 100 caracters")]
        public string Description { get; set; }
        [Required(ErrorMessage ="El estado es Requerido")]
        public bool State {  get; set; }
    }
}
