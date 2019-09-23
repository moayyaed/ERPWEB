using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.RRHH
{
    public class ro_PrestamoMasivo_Det_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public decimal IdCarga { get; set; }
        public int Secuencia { get; set; }
        public Nullable<decimal> IdPrestamo { get; set; }
        public decimal IdEmpleado { get; set; }
        public string IdRubro { get; set; }
        public double Monto { get; set; }
        public int NumCuotas { get; set; }
    }
}
