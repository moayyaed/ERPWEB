using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Facturacion
{
    public class fa_factura_tipo_Info
    {
        public int IdEmpresa { get; set; }
        public int IdFacturaTipo { get; set; }
        [Required(ErrorMessage = "El campo código es obligatorio")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public string IdUsuarioCreacion { get; set; }
        [Required(ErrorMessage = "El campo motivo de anulación es obligatorio")]
        public string MotivoAnulacion { get; set; }

        #region Campos que no existen en la tabla
        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
        #endregion
    }
}
