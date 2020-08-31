using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.CuentasPorCobrar
{
    public class cxc_CatalogoTipo_Info
    {
        public decimal IdTransaccionSession { get; set; }
        [Required(ErrorMessage = ("el campo código es obligatorio"))]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "el campo código debe tener mínimo 1 caracter y máximo 20")]
        public string IdCatalogo_tipo { get; set; }

        [Required(ErrorMessage = ("el campo descripción es obligatorio"))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 50")]
        public string Descripcion { get; set; }

        #region Campos que no existen en la tabla
        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
        #endregion
    }
}
