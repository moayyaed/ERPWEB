using Core.Erp.Info.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.CuentasPorCobrar
{
    public class cxc_LiquidacionRetProv_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public decimal IdLiquidacion { get; set; }
        [Required(ErrorMessage = "El campo fecha es obligatorio")]
        public System.DateTime li_Fecha { get; set; }
        public string Observacion { get; set; }
        public bool Estado { get; set; }
        public Nullable<int> IdTipoCbte { get; set; }
        public Nullable<decimal> IdCbteCble { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        [Required(ErrorMessage = "El campo motivo de anulación es obligatorio")]
        public string MotivoAnulacion { get; set; }

        #region Campos que no existen en la tabla
        public int IdSucursal { get; set; }
        public List<cxc_LiquidacionRetProvDet_Info> lst_detalle { get; set; }
        public List<ct_cbtecble_det_Info> lst_detalle_cbte { get; set; }
        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
        #endregion
    }
}
