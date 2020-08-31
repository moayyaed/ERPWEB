using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.CuentasPorCobrar
{
    public class cxc_MotivoLiquidacionTarjeta_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public decimal IdMotivo { get; set; }
        [Required(ErrorMessage = ("el campo descripción es obligatorio"))]
        [StringLength(10000, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 10000")]
        public string Descripcion { get; set; }
        public bool ESRetenIVA { get; set; }
        public bool ESRetenFTE { get; set; }
        public double Porcentaje { get; set; }
        public bool Estado { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        [Required(ErrorMessage = "El campo motivo anulación es obligatorio")]
        public string MotivoAnulacion { get; set; }

        public List<cxc_MotivoLiquidacionTarjeta_x_tb_sucursal_Info> Lst_det { get; set; }

        #region Campos que no existen en la tabla
        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
        #endregion
    }
}
