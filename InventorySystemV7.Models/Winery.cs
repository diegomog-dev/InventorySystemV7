using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.Models
{
    public class Winery
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(60, ErrorMessage = "Debe ser máximo de 60 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(100, ErrorMessage = "Debe ser máximo de 100 caracteres")]
        public string Description { get; set; }
        [Required(ErrorMessage ="El estado es requerido")]
        public bool State {  get; set; }
    }
}
