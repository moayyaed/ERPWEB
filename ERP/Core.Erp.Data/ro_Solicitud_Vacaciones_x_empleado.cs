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
    
    public partial class ro_Solicitud_Vacaciones_x_empleado
    {
        public ro_Solicitud_Vacaciones_x_empleado()
        {
            this.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado = new HashSet<ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado>();
        }
    
        public int IdEmpresa { get; set; }
        public int IdSolicitud { get; set; }
        public decimal IdEmpleado { get; set; }
        public string IdEstadoAprobacion { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.DateTime Fecha_Desde { get; set; }
        public System.DateTime Fecha_Hasta { get; set; }
        public System.DateTime Fecha_Retorno { get; set; }
        public string Observacion { get; set; }
        public string IdUsuario { get; set; }
        public string IdUsuario_Anu { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public string Estado { get; set; }
        public string MotivoAnulacion { get; set; }
        public bool Gozadas { get; set; }
    
        public virtual ICollection<ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado> ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado { get; set; }
        public virtual ro_empleado ro_empleado { get; set; }
    }
}
