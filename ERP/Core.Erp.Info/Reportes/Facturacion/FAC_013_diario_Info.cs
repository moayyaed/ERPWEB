using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Facturacion
{
    public class FAC_013_diario_Info
    {
        public int vt_IdEmpresa { get; set; }
        public int vt_IdSucursal { get; set; }
        public int vt_IdBodega { get; set; }
        public decimal vt_IdCbteVta { get; set; }
        public int ct_IdEmpresa { get; set; }
        public int ct_IdTipoCbte { get; set; }
        public decimal ct_IdCbteCble { get; set; }
        public int secuencia { get; set; }
        public string IdCtaCble { get; set; }
        public string IdCentroCosto { get; set; }
        public string pc_Cuenta { get; set; }
        public string cc_Descripcion { get; set; }
        public double dc_Valor { get; set; }
        public double dc_Valor_Debe { get; set; }
        public double dc_Valor_Haber { get; set; }
    }
}
