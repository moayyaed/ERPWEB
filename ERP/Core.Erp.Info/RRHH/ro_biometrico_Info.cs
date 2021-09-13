using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.RRHH
{
    public class ro_biometrico_Info
    {
        public int IdEmpresa { get; set; }
        public int IdBiometrico { get; set; }
        public int IdEquipo { get; set; }
        public string Descripcion { get; set; }
        public string StringConexion { get; set; }
        public string Consulta { get; set; }
        public bool MarcacionIngreso { get; set; }
        public bool MarcacionSalida { get; set; }
        public bool SalidaLounch { get; set; }
        public bool RegresoLounch { get; set; }
        public string CodMarcacionIngreso { get; set; }
        public string CodMarcacionSalida { get; set; }
        public string CodSalidaLounch { get; set; }
        public string CodRegresoLounch { get; set; }
        public Nullable<bool> Estado { get; set; }
        public string IdUsuario { get; set; }
        public System.DateTime Fecha_Transac { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
    }
}
