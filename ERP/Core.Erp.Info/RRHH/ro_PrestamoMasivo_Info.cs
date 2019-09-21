using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.RRHH
{
    public class ro_PrestamoMasivo_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public decimal IdCarga { get; set; }
        public System.DateTime Fecha_PriPago { get; set; }
        public double MontoSol { get; set; }
        public int NumCuotas { get; set; }
        public bool descuento_mensual { get; set; }
        public bool descuento_quincena { get; set; }
        public bool descuento_men_quin { get; set; }
        public string Observacion { get; set; }
        public bool Estado { get; set; }
        public string IdUsuario { get; set; }
        public System.DateTime Fecha_Transac { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        public string MotiAnula { get; set; }
    }
}
