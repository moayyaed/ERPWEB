using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.CuentasPorCobrar
{
    public class CXC_006_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public decimal IdCobro { get; set; }
        public int secuencial { get; set; }
        public string vt_tipoDoc { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCbteVta { get; set; }
        public string Ve_Vendedor { get; set; }
        public System.DateTime vt_fecha { get; set; }
        public Nullable<decimal> NumCotizacion { get; set; }
        public Nullable<decimal> NumOPr { get; set; }
        public string CodCbteVta { get; set; }
        public decimal IdCliente { get; set; }
        public string pe_nombreCompleto { get; set; }
        public int IdVendedor { get; set; }
        public string vt_NumFactura { get; set; }
        public System.DateTime vt_fech_venc { get; set; }
        public System.DateTime cr_fecha { get; set; }
        public Nullable<int> DiasAtraso { get; set; }
        public double ValorPago { get; set; }
        public Nullable<double> BaseComision { get; set; }
        public string IdCobro_tipo { get; set; }
    }
}
