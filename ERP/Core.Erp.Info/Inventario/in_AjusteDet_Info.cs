using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Inventario
{
    public class in_AjusteDet_Info
    {
        public int IdEmpresa { get; set; }
        public decimal IdAjuste { get; set; }
        public int Secuencia { get; set; }
        public decimal IdProducto { get; set; }
        public string IdUnidadMedida { get; set; }
        public double StockSistema { get; set; }
        public double StockFisico { get; set; }
        public double Ajuste { get; set; }
        public double Costo { get; set; }
    }
}
