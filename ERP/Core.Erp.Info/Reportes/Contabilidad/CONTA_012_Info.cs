using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Contabilidad
{
    public class CONTA_012_Info
    {
        public int IdEmpresa { get; set; }
        public Nullable<int> nivel { get; set; }
        public Nullable<int> orden { get; set; }
        public int IdTipo_Gasto { get; set; }
        public Nullable<int> orden_tipo_gasto { get; set; }
        public string nom_tipo_Gasto { get; set; }
        public string nom_tipo_Gasto_padre { get; set; }
        public double dc_Valor { get; set; }
        public string IdCta { get; set; }
        public string nom_cuenta { get; set; }
        public string nom_grupo_CC { get; set; }
    }
}
