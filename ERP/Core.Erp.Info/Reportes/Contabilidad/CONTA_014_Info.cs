using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Contabilidad
{
    public class CONTA_014_Info
    {
        public int IdEmpresa { get; set; }
        public string IdCentroCosto { get; set; }
        public string cc_Descripcion { get; set; }
        public string gc_GrupoCble { get; set; }
        public double gc_Orden { get; set; }
        public decimal ValorMostrar { get; set; }
        public decimal ValorReal { get; set; }
    }
}
