using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.CuentasPorCobrar
{
    public class cxc_LiquidacionRetProvDet_Info
    {
        public int IdEmpresa { get; set; }
        public decimal IdLiquidacion { get; set; }
        public int Secuencia { get; set; }
        public int IdSucursal { get; set; }
        public decimal IdCobro { get; set; }
        public int secuencial { get; set; }
        public string IdCobro_tipo { get; set; }
        public double Valor { get; set; }

        #region Campos que no existen en la tabla
        public double dc_ValorPago { get; set; }
        public string tc_descripcion { get; set; }
        public System.DateTime cr_fecha { get; set; }
        public string cr_observacion { get; set; }
        public Nullable<bool> cr_EsProvision { get; set; }
        public string cr_estado { get; set; }
        public string IdCtaCble { get; set; }
        public string pc_Cuenta { get; set; }
        public string ESRetenIVA { get; set; }
        public string ESRetenFTE { get; set; }
        public string cr_NumDocumento { get; set; }
        #endregion
    }
}
