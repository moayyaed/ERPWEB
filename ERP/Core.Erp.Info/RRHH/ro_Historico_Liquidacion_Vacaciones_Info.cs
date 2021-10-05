using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Core.Erp.Info.RRHH
{
   public class ro_Historico_Liquidacion_Vacaciones_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdLiquidacion { get; set; }
        public decimal IdEmpleado { get; set; }
        public int IdSolicitud { get; set; }
        public Nullable<decimal> IdOrdenPago { get; set; }
        public Nullable<int> IdEmpresa_OP { get; set; }
        public string IdTipo_op { get; set; }
        public double ValorCancelado { get; set; }
        public System.DateTime FechaPago { get; set; }
        public string Observaciones { get; set; }
        public string IdUsuario { get; set; }
        public string Estado { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> FechaHoraAnul { get; set; }
        public string MotiAnula { get; set; }
        public string IdUsuarioUltAnu { get; set; }



        public string IdEstadoAprobacion { get; set; }
        public System.DateTime Fecha { get; set; }
        public Nullable<System.DateTime> Fecha_Desde { get; set; }
        public Nullable<System.DateTime> Fecha_Hasta { get; set; }
        public Nullable<System.DateTime> Fecha_Retorno { get; set; }
        public bool Gozadas { get; set; }
        public string pe_nombre_completo { get; set; }
        public bool EstadoBool { get; set; }

        public List<ro_Historico_Liquidacion_Vacaciones_Det_Info> lst_detalle { get; set; }
        public List<ro_Solicitud_Vacaciones_x_empleado_det_Info> lst_periodos { get; set; }

        public ro_Historico_Liquidacion_Vacaciones_Info()
        {
            lst_detalle = new List<ro_Historico_Liquidacion_Vacaciones_Det_Info>();
            lst_periodos = new List<ro_Solicitud_Vacaciones_x_empleado_det_Info>();
        }
    }
}
