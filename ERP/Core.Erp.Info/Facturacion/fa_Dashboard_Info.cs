using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Facturacion
{
    public class fa_Dashboard_Info
    {
        public int Anio { get; set; }
        public decimal Total { get; set; }
        public decimal Precio { get; set; }
        public string Mes { get; set; }
        public string Precio_String { get; set; }
        public string Total_String { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal TotalCobro { get; set; }
        public decimal TotalPagos { get; set; }
        public string FechaIni_String { get; set; }
        public string FechaFin_String { get; set; }
        public string TotalFactura_String { get; set; }
        public string TotalCobro_String { get; set; }
        public string TotalPagos_String { get; set; }
    }
}
