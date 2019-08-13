using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Compras
{
    public class com_ordencompra_local_resumen_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public decimal IdOrdenCompra { get; set; }
        public decimal SubtotalIVASinDscto { get; set; }
        public decimal SubtotalSinIVASinDscto { get; set; }
        public decimal SubtotalSinDscto { get; set; }
        public decimal Descuento { get; set; }
        public decimal SubtotalIVAConDscto { get; set; }
        public decimal SubtotalSinIVAConDscto { get; set; }
        public decimal SubtotalConDscto { get; set; }
        public decimal ValorIVA { get; set; }
        public decimal Total { get; set; }
    }
}
