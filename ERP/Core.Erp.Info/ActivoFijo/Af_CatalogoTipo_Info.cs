using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.ActivoFijo
{
    public class Af_CatalogoTipo_Info
    {
        public decimal IdTransaccionSession { get; set; }
        [Required(ErrorMessage = ("el campo código es obligatorio"))]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "el campo código debe tener mínimo 1 caracter y máximo 25")]
        public string IdTipoCatalogo { get; set; }
        [Required(ErrorMessage = ("el campo descripción es obligatorio"))]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 100")]
        public string Descripcion { get; set; }

        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
    }
}
