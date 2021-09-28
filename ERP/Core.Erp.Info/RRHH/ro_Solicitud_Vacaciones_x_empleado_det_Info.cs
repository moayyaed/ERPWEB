using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.RRHH
{
   public class ro_Solicitud_Vacaciones_x_empleado_det_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSolicitud { get; set; }
        public int Secuencia { get; set; }
        public decimal IdEmpleado { get; set; }
        public int IdPeriodo_Inicio { get; set; }
        public int IdPeriodo_Fin { get; set; }
        public string Observacion { get; set; }
        public int Dias_tomados { get; set; }
        public string Tipo_liquidacion { get; set; }

        public System.DateTime FechaIni { get; set; }
        public System.DateTime FechaFin { get; set; }
    }
}
