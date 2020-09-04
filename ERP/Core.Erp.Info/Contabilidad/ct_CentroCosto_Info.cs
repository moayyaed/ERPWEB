using DevExpress.ExpressApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Contabilidad
{
    public class ct_CentroCosto_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public string IdCentroCosto { get; set; }
        public string IdCentroCostoPadre { get; set; }
        public int IdNivel { get; set; }
        public string cc_Descripcion { get; set; }
        public bool EsMovimiento { get; set; }
        public bool Estado { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        [Required(ErrorMessage = "El campo motivo de anulación es obligatorio")]
        public string MotivoAnulacion { get; set; }


        #region Campos que no estan en la tabla
        public string cc_Descripcion_Padre { get; set; }
        public string nv_Descripcion { get; set; }
        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
        #endregion

    }
}
