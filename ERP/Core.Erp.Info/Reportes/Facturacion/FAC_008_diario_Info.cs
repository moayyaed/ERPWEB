using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Facturacion
{
    public class FAC_008_diario_Info
    {
        public int no_IdEmpresa { get; set; }
        public int no_IdSucursal { get; set; }
        public int no_IdBodega { get; set; }
        public decimal no_IdNota { get; set; }
        public int ct_IdEmpresa { get; set; }
        public int ct_IdTipoCbte { get; set; }
        public decimal ct_IdCbteCble { get; set; }
        public int secuencia { get; set; }
        public string IdCtaCble { get; set; }
        public Nullable<int> IdPunto_cargo_grupo { get; set; }
        public Nullable<int> IdPunto_cargo { get; set; }
        public string IdCentroCosto { get; set; }
        public string cc_Descripcion { get; set; }
        public string nom_punto_cargo { get; set; }
        public string nom_punto_cargo_grupo { get; set; }
        public string pc_Cuenta { get; set; }
        public double dc_Valor { get; set; }
        public double dc_Valor_Debe { get; set; }
        public double dc_Valor_Haber { get; set; }
    }
}
