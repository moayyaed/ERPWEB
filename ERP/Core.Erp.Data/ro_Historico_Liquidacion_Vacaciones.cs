//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Erp.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ro_Historico_Liquidacion_Vacaciones
    {
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
    
        public virtual ro_empleado ro_empleado { get; set; }
    }
}
