using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Inventario
{
    public class in_CatalogoTipo_Info
    {
        [Key]
        public decimal IdTransaccionSession { get; set; }
        public int IdCatalogo_tipo { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = ("El campo código debe tener mínimo 1 caracter máximo 50"))]
        public string cod_Catalogo_tipo { get; set; }

        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = ("El campo descripción debe tener mínimo 1 caracter máximo 50"))]
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public bool EstadoBool { get; set; }


        #region Campos que no existen en la tabla
        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
        #endregion
    }
}
