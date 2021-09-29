using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.RRHH
{
  public  class ro_empleado_x_jefes_inmediatos_Info
    {
        public int IdEmpresa { get; set; }
        public decimal IdEmpleado { get; set; }
        public int Secuencia { get; set; }
        public decimal IdEmpleado_aprueba { get; set; }
        public bool Aprueba_vacaciones { get; set; }
        public bool Aprueba_prestamo { get; set; }
    }
}
