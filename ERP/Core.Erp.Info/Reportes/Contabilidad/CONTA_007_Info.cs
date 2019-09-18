using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Contabilidad
{
    public class CONTA_007_Info
    {
        public int IdEmpresa { get; set; }
        public string IdUsuario { get; set; }
        public string Secuencia { get; set; }
        public string IdCtaCble { get; set; }
        public string pc_cuenta { get; set; }
        public string Tipo { get; set; }
        public int TipoOrden { get; set; }
        public string Clasificacion { get; set; }
        public int ClasificacionOrden { get; set; }
        public decimal Valor { get; set; }
    }
}
