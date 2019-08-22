using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.General
{
    public class tb_bodega_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }        
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "el campo código debe tener máximo 50 caracteres")]
        public string cod_bodega { get; set; }
        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 200")]
        public string bo_Descripcion { get; set; }
        public Nullable<bool> bo_EsBodega { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        public string Estado { get; set; }
        public string IdCtaCtble_Inve { get; set; }
        

        #region Campos que no existen en la tabla
        public string IdString { get; set; }
        public bool EstadoBool { get; set; }
        #endregion

    }
}
