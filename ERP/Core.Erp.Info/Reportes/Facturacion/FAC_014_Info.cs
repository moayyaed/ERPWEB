using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Facturacion
{
    public class FAC_014_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCbteVta { get; set; }
        public DateTime vt_fecha { get; set; }
        public decimal IdCliente { get; set; }
        public string NumCotizacion { get; set; }
        public string NumOPr { get; set; }
        public string vt_NumFactura { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string pr_descripcion { get; set; }
        public string IdCentroCosto { get; set; }
        public double vt_Subtotal { get; set; }
        public string vt_Observacion { get; set; }
        public string cc_Descripcion { get; set; }
    }
}
