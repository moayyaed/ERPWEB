using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La descripción debe tener mínimo 4 caracteres y máximo 100")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo string conexión es obligatorio")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "La descripción debe tener mínimo 4 caracteres y máximo 500")]
        public string StringConexion { get; set; }
        [Required(ErrorMessage = "El campo consulta es obligatorio")]
        public string Consulta { get; set; }
        public bool MarcacionIngreso { get; set; }
        public bool MarcacionSalida { get; set; }
        public bool SalidaLounch { get; set; }
        public bool RegresoLounch { get; set; }
        [Required(ErrorMessage = "El campo codigo marcacion ingreso es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "La descripción debe tener mínimo 4 caracteres y máximo 50")]
        public string CodMarcacionIngreso { get; set; }
        [Required(ErrorMessage = "El campo codigo marcacion salida es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "La descripción debe tener mínimo 4 caracteres y máximo 50")]
        public string CodMarcacionSalida { get; set; }
        [Required(ErrorMessage = "El campo codigo marcacion salida lountch es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "La descripción debe tener mínimo 4 caracteres y máximo 50")]
        public string CodSalidaLounch { get; set; }
        [Required(ErrorMessage = "El campo codigo marcacion regreso lountch es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "La descripción debe tener mínimo 4 caracteres y máximo 50")]
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
