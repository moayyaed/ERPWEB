using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.RRHH
{
   public class ro_canasta_basica_Info
    {
        public int Anio { get; set; }
        public decimal valorCanasta { get; set; }
        public double MultiploCanastaBasica { get; set; }
        public double MultiploFraccionBasica { get; set; }
        public string Observacion { get; set; }
        public string IdUsuario { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> FechaTansaccion { get; set; }
        public Nullable<System.DateTime> FechaUltModi { get; set; }

    }
}
