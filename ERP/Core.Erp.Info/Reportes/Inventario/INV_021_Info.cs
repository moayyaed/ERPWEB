using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Inventario
{
    public class INV_021_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdMovi_inven_tipo { get; set; }
        public decimal IdNumMovi { get; set; }
        public string tm_descripcion { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string signo { get; set; }
        public System.DateTime cm_fecha { get; set; }
        public string cm_observacion { get; set; }
        public string Estado { get; set; }
        public Nullable<int> IdBodega { get; set; }
        public string bo_Descripcion { get; set; }
        public string Su_Descripcion { get; set; }
        public string CodMoviInven { get; set; }
        public string Desc_mov_inv { get; set; }
        public Nullable<int> IdMotivo_Inv { get; set; }
        public string IdEstadoAproba { get; set; }
        public string EstadoAprobacion { get; set; }
        public string co_factura { get; set; }
        public Nullable<System.DateTime> co_FechaContabilizacion { get; set; }
    }
}
