using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Contabilidad
{
    public class ct_ClasificacionEBIT_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdClasificacionEBIT { get; set; }
        public string ebit_Codigo { get; set; }
        public string ebit_Descripcion { get; set; }
        public bool AplicaEBIT { get; set; }
        public bool AplicaEBITDA { get; set; }

        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
    }
}
